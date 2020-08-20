using Distributor;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FuncDistribution
{
    public  class FuncDistribute
    {
        private readonly IEnumerable<IDistributor> _distributors;
        private readonly ILogger<FuncDistribute> _log;

        public FuncDistribute(IEnumerable<IDistributor> distributors,
                              ILogger<FuncDistribute> log)
        {
            _distributors = distributors;
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

                foreach(var distributor in _distributors)
                {
                    distributor.Distribute(emailTo, comicItemTitle, message);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, ", Error: " + e.Message);
                throw;
            }
        }
    }
}  
