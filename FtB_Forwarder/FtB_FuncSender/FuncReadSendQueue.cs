using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace FtB_FuncSender
{
    public class FuncReadSendQueue
    {
        private readonly SendQueueProcessor _queueProcessor;

        public FuncReadSendQueue(SendQueueProcessor queueProcessor)
        {
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSendQueue")]
        public void Run([ServiceBusTrigger("%SendingQueueName%", Connection = "queueConnectionString")] string myQueueItem, ILogger log,
            [ServiceBus("%ReportQueueName%", Connection = "queueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<ReportQueueItem> queueCollector)
        {
            SendQueueItem sendQueueItem = JsonConvert.DeserializeObject<SendQueueItem>(myQueueItem);
            log.LogInformation($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: C# ServiceBus queue trigger function processed message: {sendQueueItem.ArchiveReference} for {sendQueueItem.Receiver.Id}");

            var result = _queueProcessor.ExecuteProcessingStrategy(sendQueueItem);
            if (result != null)
            {
                queueCollector.AddAsync(result);
            }
        }
    }
}
