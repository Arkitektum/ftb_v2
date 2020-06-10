using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FtB_PreProsessor.OutboundQueue
{
    /// <summary>
    /// Integration with the queue system, i.e. Azure Service Bus
    /// </summary>
    public class QueueClient : IQueueClient
    {
        private readonly ILogger<QueueClient> _logger;
        private readonly IServiceBusQueueClientFactory _serviceBusQueueClientFactory;
        const string QueueName = "ftbpoc2";

        public  QueueClient(ILogger<QueueClient> logger, IServiceBusQueueClientFactory serviceBusQueueClientFactory)
        {
            _logger = logger;
            _serviceBusQueueClientFactory = serviceBusQueueClientFactory;
        }
        public void QueueFormForProcessing(QueueMessage queueMessage)
        {
            try
            {
                var queueClient =  _serviceBusQueueClientFactory.GetQueueClientFor(QueueName).Result;
                
                _logger.LogInformation("Queued form for further processing {0}", queueMessage.Reference);
                var payload = JsonConvert.SerializeObject(queueMessage);
                var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(payload));                
                 queueClient.SendAsync(message);
                
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error occurred while communicating with Azure Service Bus");
                throw;
            }
            
        }
    }
}
