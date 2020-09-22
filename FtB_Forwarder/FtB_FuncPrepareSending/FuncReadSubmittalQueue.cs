using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FtB_Common.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtB_FuncPrepareSending
{
    public class FuncReadSubmittalQueue
    {
        private readonly ArchivedItemQueueProcessor _queueProcessor;

        public FuncReadSubmittalQueue(ArchivedItemQueueProcessor queueProcessor)
        {
            Debug.WriteLine("Method FuncReadSubmittalQueue");
            _queueProcessor = queueProcessor;
        }

        [FunctionName("FuncReadSubmittalQueue")]
        public void Run([ServiceBusTrigger("%SubmittalQueueName%", Connection = "queueConnectionString")]string myQueueItem, ILogger log,
            [ServiceBus("%SendingQueueName%", Connection = "queueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<SendQueueItem> queueCollector)
        {
            Debug.WriteLine("Method Run");
            SubmittalQueueItem queueItemJson = JsonConvert.DeserializeObject<SubmittalQueueItem>(myQueueItem);
            string archiveReference = queueItemJson.ArchiveReference;
            //_queueProcessor.ExecuteProcessingStrategy(archiveReference);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var result = _queueProcessor.ExecuteProcessingStrategy(archiveReference);

            var tasks = new List<Task>();

            foreach (var item in result)
            {
                tasks.Add(queueCollector.AddAsync(item));
            }

            Task.WhenAll(tasks);
        }

        //[FunctionName("HttpTriggerCSharp")]
        //public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        //                                            HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    return name != null
        //        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        //        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        //}
    }
}
