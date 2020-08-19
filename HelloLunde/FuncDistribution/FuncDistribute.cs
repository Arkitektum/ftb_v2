using System;
using Distributor;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public void Run([ServiceBusTrigger("%queueName%"
                        , Connection = "queueConnectionString")]string myQueueItem)
        {
            try
            {
                dynamic queueMessage = JsonConvert.DeserializeObject(myQueueItem);
                string emailTo = queueMessage.emailTo;
                string comicItemTitle = queueMessage.comicItem.Safe_Title;
                string message = queueMessage.comicItem.Transcript 
                                + System.Environment.NewLine 
                                + System.Environment.NewLine 
                                + queueMessage.comicItem.Img;

                _log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                _distributor.Distribute(emailTo, comicItemTitle, message);
            }
            catch (Exception e)
            {
                _log.LogError(e, ", Error: " + e.Message);
                throw;
            }
        }
    }
}  
