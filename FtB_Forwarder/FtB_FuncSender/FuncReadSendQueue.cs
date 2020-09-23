using FtB_Common.BusinessModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FuncSender
{
    public class FuncReadSendQueue
    {
        private readonly SendQueueProcessor _queueProcessor;

        public FuncReadSendQueue(SendQueueProcessor queueProcessor)
        {
            _queueProcessor = queueProcessor;
        }

        /// <summary>
        /// Read from %SendingQueueName%, and put a ReportQueueItem on the %ReportQueueName% when all sendings are processed
        /// This requires to persist status of sending for an archiveReference in some place
        /// </summary>
        /// <param name="myQueueItem"></param>
        /// <param name="log"></param>
        /// <param name="queueCollector"></param>
        [FunctionName("FuncReadSendQueue")]
        public void Run([ServiceBusTrigger("%SendingQueueName%", Connection = "queueConnectionString")] string myQueueItem, ILogger log,
            [ServiceBus("%ReportQueueName%", Connection = "queueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<ReportQueueItem> queueCollector)
        {
            Debug.WriteLine("Method FuncReadSendQueue.Run");
            SendQueueItem sendQueueItem = JsonConvert.DeserializeObject<SendQueueItem>(myQueueItem);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var result = _queueProcessor.ExecuteProcessingStrategy(sendQueueItem);

            var tasks = new List<Task>();

            foreach (var item in result)
            {
                tasks.Add(queueCollector.AddAsync(item));
            }

            Task.WhenAll(tasks);
        }
    }
}
