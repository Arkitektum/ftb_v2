using MetadataOrchestrator;
using MetadataProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FuncEnqueueMetadata
{
    public class FuncEnqueueMetadata
    {
        private readonly IOrchestrator _orchestrator;

        public FuncEnqueueMetadata(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }


        [FunctionName("FuncEnqueueMetadata")]
        public  async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log,
            [ServiceBus("HelloLunde", Connection = "queueConnectionString", EntityType = EntityType.Queue)] IAsyncCollector<MetadataItem> queueCollector)           
            
        {
            log.LogInformation("FuncEnqueuMetadata triggered");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            
            dynamic requestBody = JsonConvert.DeserializeObject(content);
            string emailTo = requestBody.emailTo;
            var result = await _orchestrator.RetreiveMetadata(emailTo);

            var tasks = new List<Task>();

            foreach (var item in result)
            {
                tasks.Add(queueCollector.AddAsync(item));                
            }

            await Task.WhenAll(tasks);
        }
    }
}
