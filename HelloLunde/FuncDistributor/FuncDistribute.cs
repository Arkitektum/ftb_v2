using System;
using Distributor;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FuncDistributor
{
    public class FuncDistribute
    {
        private readonly IDistributor _distributor;
        public FuncDistribute(IDistributor distributor)
        {
            _distributor = distributor;
        }
        [FunctionName("FuncDistribute")]
        public void Run([ServiceBusTrigger("myqueue", Connection = "")]string myQueueItem, ILogger log)
        {
            //log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            _distributor.Distribute("sdads");
        }
    }
}
