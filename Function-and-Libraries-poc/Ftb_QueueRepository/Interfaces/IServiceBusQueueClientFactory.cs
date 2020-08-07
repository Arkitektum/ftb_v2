using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ftb_QueueRepository
{
    public interface IServiceBusQueueClientFactory
    {
        Task<Microsoft.Azure.ServiceBus.IQueueClient> GetQueueClientFor(string queueName);
    }
}
