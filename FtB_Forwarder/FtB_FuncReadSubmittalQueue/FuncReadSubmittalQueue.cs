using System;
using FtB_InitiateForwarding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FtB_FuncForwarding
{
    public class FuncReadSubmittalQueue
    {
        private readonly ArchivedItemQueueProcessor _queueProcessor;

        public FuncReadSubmittalQueue(ArchivedItemQueueProcessor queueProcessor)
        {
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSubmittalQueue")]
        public void Run([ServiceBusTrigger("%queueName%", Connection = "queueConnectionString")]string myQueueItem, ILogger log)
        {
            string archiveReference = "ar45555";
            _queueProcessor.ExecuteProcessingStrategy(archiveReference);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
