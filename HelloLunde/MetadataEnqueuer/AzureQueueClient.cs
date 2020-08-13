using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MetadataEnqueuer
{
    public class AzureQueueClient : IQueueClient
    {
        private readonly IOptions<QueueSettings> _options;

        public AzureQueueClient(IOptions<QueueSettings> options)
        {
            if (string.IsNullOrEmpty(options?.Value?.ConnectionString) || string.IsNullOrEmpty(options?.Value?.QueueName))
                throw new ArgumentException("Ensure that QueueSettings is configured");

            _options = options;
        }
        public async Task EnqueueMessage(string messageAsJson)
        {
            var queueClient = await GetQueueClient();

            var payload = messageAsJson;
            var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(payload));
            await queueClient.SendAsync(message);
        }

        public async Task<Microsoft.Azure.ServiceBus.IQueueClient> GetQueueClient()
        {
            var managementClient = new Microsoft.Azure.ServiceBus.Management.ManagementClient(_options.Value.ConnectionString);

            try
            {
                var queue = await managementClient.GetQueueAsync(_options.Value.QueueName);
            }
            catch (Microsoft.Azure.ServiceBus.MessagingEntityNotFoundException)
            {
                await managementClient.CreateQueueAsync(new Microsoft.Azure.ServiceBus.Management.QueueDescription(_options.Value.QueueName) { EnablePartitioning = true });
            }

            return new Microsoft.Azure.ServiceBus.QueueClient(_options.Value.ConnectionString, _options.Value.QueueName);
        }
    }
}
