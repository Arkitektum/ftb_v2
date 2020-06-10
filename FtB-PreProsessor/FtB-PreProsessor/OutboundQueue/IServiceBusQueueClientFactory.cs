using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_PreProsessor.OutboundQueue
{
    public interface IServiceBusQueueClientFactory
    {
        Task<Microsoft.Azure.ServiceBus.IQueueClient> GetQueueClientFor(string queueName);
    }
}
