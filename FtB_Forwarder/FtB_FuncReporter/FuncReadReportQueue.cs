using FtB_Common.BusinessModels;
using FtB_ProcessStrategies;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        public void Run([ServiceBusTrigger("%ReportQueueName%", Connection = "QueueConnectionString")] string myQueueItem)
        {
            ReportQueueItem reportQueueItem = JsonConvert.DeserializeObject<ReportQueueItem>(myQueueItem);
            using (var scope = _logger.BeginScope(new Dictionary<string, string> { { "ArchiveReference", reportQueueItem.ArchiveReference }, { "ReceiverId", reportQueueItem.Receiver.Id } }))
            {
                _logger.LogInformation($"{reportQueueItem.Receiver.Id }: C# ServiceBus queue trigger function processed message: {myQueueItem}");
                var result = _queueProcessor.ExecuteProcessingStrategy(reportQueueItem);
            }
        }
    }
}
