using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System.Threading.Tasks;

namespace FtB_PreProsessor.OutboundQueue
{
    public class ServiceBusQueueClientFactory : IServiceBusQueueClientFactory
    {
        const string serviceBusConnectionString = "Endpoint=sb://ftb-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pcgBRJia0MKp7pELy8W5rBaIQMeZyKVnH0h3gdhROzA=";
        public async Task<Microsoft.Azure.ServiceBus.IQueueClient> GetQueueClientFor(string queueName)
        {
            var managementClient = new Microsoft.Azure.ServiceBus.Management.ManagementClient(serviceBusConnectionString);

            try
            {
                var queue = await managementClient.GetQueueAsync(queueName);

                
            }
            catch (MessagingEntityNotFoundException)
            {
                await managementClient.CreateQueueAsync(new QueueDescription(queueName) { EnablePartitioning = true });
            }

            return new Microsoft.Azure.ServiceBus.QueueClient(serviceBusConnectionString, queueName);
        }

    }
}
