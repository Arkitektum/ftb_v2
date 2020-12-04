﻿using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_FuncReporter
{
    public class FuncReadReportQueue
    {
        private readonly ILogger<FuncReadReportQueue> _logger;
        private readonly ReportQueueProcessor _queueProcessor;

        public FuncReadReportQueue(ILogger<FuncReadReportQueue> logger, ReportQueueProcessor queueProcessor)
        {
            this._logger = logger;
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadReportQueue")]
        public async Task Run([ServiceBusTrigger("%ReportQueueName%", Connection = "QueueConnectionString")] Message message, MessageReceiver messageReceiver, string lockToken)
        {
            var myQueueItem = System.Text.Encoding.Default.GetString(message.Body);
            ReportQueueItem reportQueueItem = JsonConvert.DeserializeObject<ReportQueueItem>(myQueueItem);
            using (var scope = _logger.BeginScope(new Dictionary<string, string> { { "ArchiveReference", reportQueueItem.ArchiveReference }, { "ReceiverId", reportQueueItem.Receiver.Id } }))
            {
                _logger.LogInformation($"{GetType().Name}: {reportQueueItem.Receiver.Id }: C# ServiceBus queue trigger function processed message: {myQueueItem}");

                try
                {
                    var result = await _queueProcessor.ExecuteProcessingStrategyAsync(reportQueueItem);
                    await messageReceiver.CompleteAsync(lockToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{GetType().Name}: Something went wrong. Exception: {ex}");
                    await messageReceiver.DeadLetterAsync(lockToken);
                    throw; //Do this to make sure the message is not removed from the SendQueue. It will retry, and then end up in DeadLetterQueue
                }
            }
        }
    }
}
