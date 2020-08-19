using MetadataOrchestrator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("FuncEnqueuMetadata triggered");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            
            dynamic requestBody = JsonConvert.DeserializeObject(content);
            string emailTo = requestBody.emailTo;
            await _orchestrator.EnqueueMetadata(emailTo);
            
            return new OkObjectResult("Great success in FuncEnqueueMetadata!");
        }
    }
}
