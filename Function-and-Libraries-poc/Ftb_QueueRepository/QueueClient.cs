using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Ftb_QueueRepository
{
    /// <summary>
    /// Integration with the queue system, i.e. Azure Service Bus
    /// </summary>
    public class QueueClient : IQueueClient
    {
        private readonly ILogger<QueueClient> _logger;
        private readonly IOptionsMonitor<ServiceBusSettings> _options; 
        private readonly IServiceBusQueueClientFactory _serviceBusQueueClientFactory;

        public QueueClient(ILogger<QueueClient> logger, IServiceBusQueueClientFactory serviceBusQueueClientFactory, IOptionsMonitor<ServiceBusSettings> options)
        {
            _logger = logger;
            _serviceBusQueueClientFactory = serviceBusQueueClientFactory;
            _options = options;
        }
        public async Task AddToQueue(IMetadataQueueItem queueItem)
        {
            try
            {
                var queueClient = _serviceBusQueueClientFactory.GetQueueClientFor(_options.CurrentValue.MetadataQueue).Result;
                _logger.LogInformation("Queued form for further processing {0}", queueItem.Reference);
                var payload = JsonConvert.SerializeObject(queueItem);
                var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(payload));                
                await queueClient.SendAsync(message);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error occurred while communicating with Azure Service Bus");
                throw;
            }
            
        }
    }
}
