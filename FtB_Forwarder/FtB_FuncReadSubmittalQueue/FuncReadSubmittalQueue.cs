using System;
using FtB_InitiateForwarding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FtB_FuncForwarding
{
    public static class FuncReadSubmittalQueue
    {
        [FunctionName("FuncReadSubmittalQueue")]
        public static void Run([ServiceBusTrigger("%queueName%", Connection = "queueConnectionString")]string myQueueItem, ILogger log)
        {
            string archiveReference = "ar45555";
            new ArchivedItemQueueProcessor(archiveReference);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
