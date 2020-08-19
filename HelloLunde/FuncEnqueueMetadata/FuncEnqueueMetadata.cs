using MetadataOrchestrator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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

            await _orchestrator.EnqueueMetadata();

            return new OkObjectResult("Great success in FuncEnqueueMetadata!");
        }
    }
}
