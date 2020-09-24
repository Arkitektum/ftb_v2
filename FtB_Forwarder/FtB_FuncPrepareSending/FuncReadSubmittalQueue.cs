using FtB_Common.BusinessModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
