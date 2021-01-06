using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
using Microsoft.Azure.ServiceBus.Core;
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
        public async Task Run([ServiceBusTrigger("%SubmittalQueueName%", Connection = "QueueConnectionString")] string myQueueItem,
            [ServiceBus("%SendingQueueName%", Connection = "QueueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<SendQueueItem> queueCollector)
        {
            SubmittalQueueItem submittalQueueItem = JsonConvert.DeserializeObject<SubmittalQueueItem>(myQueueItem);

            using (var scope = _logger.BeginScope(new Dictionary<string, string> { { "ArchiveReference", submittalQueueItem.ArchiveReference } }))
            {
                _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {submittalQueueItem.ArchiveReference}");

                var results = await _queueProcessor.ExecuteProcessingStrategy(submittalQueueItem);

                if (results != null)
                {
                    var tasks = new List<Task>();

                    foreach (var item in results)
                    {
                        tasks.Add(queueCollector.AddAsync(item));
                    }
                    await Task.WhenAll(tasks);
                    //await queueCollector.FlushAsync();                }
                }
            }
        }
    }
}
