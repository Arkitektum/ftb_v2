using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FtB_Common.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtB_FuncPrepareSending
{
    public class FuncReadSubmittalQueue
    {
        private readonly SubmittalQueueProcessor _queueProcessor;

        public FuncReadSubmittalQueue(SubmittalQueueProcessor queueProcessor)
        {
            Debug.WriteLine("Method FuncReadSubmittalQueue");
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSubmittalQueue")]
        public void Run([ServiceBusTrigger("%SubmittalQueueName%", Connection = "queueConnectionString")]string myQueueItem, ILogger log,
            [ServiceBus("%SendingQueueName%", Connection = "queueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<SendQueueItem> queueCollector)
        {
            Debug.WriteLine("Method FuncReadSubmittalQueue.Run");
            SubmittalQueueItem submittalQueueItem = JsonConvert.DeserializeObject<SubmittalQueueItem>(myQueueItem);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var result = _queueProcessor.ExecuteProcessingStrategy(submittalQueueItem);

            var tasks = new List<Task>();

            foreach (var item in result)
            {
                tasks.Add(queueCollector.AddAsync(item));
            }

            Task.WhenAll(tasks);
        }
    }
}
