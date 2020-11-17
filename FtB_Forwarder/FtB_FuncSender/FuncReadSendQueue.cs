using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FtB_FuncSender
{
    public class FuncReadSendQueue
    {
        private readonly ILogger<FuncReadSendQueue> _logger;
        private readonly SendQueueProcessor _queueProcessor;

        public FuncReadSendQueue(ILogger<FuncReadSendQueue> logger, SendQueueProcessor queueProcessor)
        {
            _logger = logger;
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSendQueue")]
        public void Run([ServiceBusTrigger("%SendingQueueName%", Connection = "QueueConnectionString")] string myQueueItem,
            [ServiceBus("%ReportQueueName%", Connection = "QueueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<ReportQueueItem> queueCollector)
        {
            SendQueueItem sendQueueItem = JsonConvert.DeserializeObject<SendQueueItem>(myQueueItem);
            
            using (var scope = _logger.BeginScope(new Dictionary<string,string> { { "ArchiveReference", sendQueueItem.ArchiveReference }, { "ReceiverId", sendQueueItem.Receiver.Id } }))
            {
                _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {sendQueueItem.ArchiveReference} for {sendQueueItem.Receiver.Id}");
                try
                {

                    var result = _queueProcessor.ExecuteProcessingStrategy(sendQueueItem);
                    if (result != null)
                    {
                        queueCollector.AddAsync(result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong. Exception: {ex}");
                    throw; //Do this to make sure the message is not removed from the SendQueue. It will retry, and then end up in DeadLetterQueue
                }
            }
        }
    }
}
