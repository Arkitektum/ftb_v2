using System;
using Distributor;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FuncDistribution
{
    public  class FuncDistribute
    {
        private readonly IDistributor _distributor;
        private readonly ILogger<FuncDistribute> _log;

        public FuncDistribute(IDistributor distributor,
                              ILogger<FuncDistribute> log)
        {
            _distributor = distributor;
            _log = log;
        }

        [FunctionName("FuncDistribute")]
        public void Run([ServiceBusTrigger("%integrationQueueName%"
                        , Connection = "integrationQueueConnectionString")]string myQueueItem)
        {
            try
            {
                string emailTo = "";
                JObject jObj = JObject.Parse(myQueueItem);
                object input = jObj["emailTo"];
                if (input != null)
                    emailTo = input.ToString();

                _log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                _distributor.Distribute(emailTo);
            }
            catch (Exception e)
            {
                _log.LogError(e, ", Error: " + e.Message);
                throw;
            }
        }
    }
}  
