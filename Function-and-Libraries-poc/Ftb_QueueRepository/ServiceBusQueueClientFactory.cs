using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Ftb_QueueRepository
{
    public class ServiceBusQueueClientFactory : IServiceBusQueueClientFactory
    {
        private readonly IOptionsMonitor<ServiceBusSettings> _options;
        public ServiceBusQueueClientFactory(IOptionsMonitor<ServiceBusSettings> options)
        {
            _options = options;
        }
        public async Task<Microsoft.Azure.ServiceBus.IQueueClient> GetQueueClientFor(string queueName)
        {
            var managementClient = new Microsoft.Azure.ServiceBus.Management.ManagementClient(_options.CurrentValue.ServiceBusConnectionString);

            try
            {
                var queue = await managementClient.GetQueueAsync(queueName);
            }
            catch (MessagingEntityNotFoundException)
            {
                await managementClient.CreateQueueAsync(new QueueDescription(queueName) { EnablePartitioning = true });
            }

            return new Microsoft.Azure.ServiceBus.QueueClient(_options.CurrentValue.ServiceBusConnectionString, queueName);
        }

    }
}
