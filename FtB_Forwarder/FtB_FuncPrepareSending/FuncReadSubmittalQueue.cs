using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
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
        private readonly ILogger<FuncReadSubmittalQueue> _logger;
        private readonly SubmittalQueueProcessor _queueProcessor;

        public FuncReadSubmittalQueue(ILogger<FuncReadSubmittalQueue> logger, SubmittalQueueProcessor queueProcessor)
        {
            Debug.WriteLine("Method FuncReadSubmittalQueue");
            _logger = logger;
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSubmittalQueue")]
        public void Run([ServiceBusTrigger("%SubmittalQueueName%", Connection = "QueueConnectionString")]string myQueueItem,
            [ServiceBus("%SendingQueueName%", Connection = "QueueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<SendQueueItem> queueCollector)
        {            
            SubmittalQueueItem submittalQueueItem = JsonConvert.DeserializeObject<SubmittalQueueItem>(myQueueItem);

            using (var scope = _logger.BeginScope("ArchiveReference: {0}", submittalQueueItem.ArchiveReference))
            {
                _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {submittalQueueItem.ArchiveReference}");

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
}
