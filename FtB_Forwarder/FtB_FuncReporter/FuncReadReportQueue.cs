using FtB_Common.BusinessModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtB_FuncReporter
{
    public class FuncReadReportQueue
    {
        private readonly ReportQueueProcessor _queueProcessor;

        public FuncReadReportQueue(ReportQueueProcessor queueProcessor)
        {
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadReportQueue")]
        public void Run([ServiceBusTrigger("%ReportQueueName%", Connection = "queueConnectionString")] string myQueueItem, ILogger log)
        {
            ReportQueueItem reportQueueItem = JsonConvert.DeserializeObject<ReportQueueItem>(myQueueItem);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            var result = _queueProcessor.ExecuteProcessingStrategy(reportQueueItem);
        }
    }
}
