using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FtB_ProcessStrategies;

namespace FtB_FuncAccumulationReporter
{
    public class FuncSvarVarselOmOppstartAvPlanarbeidReporter
    {
        private readonly ILogger<FuncSvarVarselOmOppstartAvPlanarbeidReporter> _logger;
        private readonly ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor _reportProcessor;

        public FuncSvarVarselOmOppstartAvPlanarbeidReporter(ILogger<FuncSvarVarselOmOppstartAvPlanarbeidReporter> logger, 
                                                            ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor processor)
        {
            _logger = logger;
            _reportProcessor = processor;
        }

        [FunctionName("ReportSvarVarselOmOppstartAvPlanarbeid")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _reportProcessor.ExecuteProcessingStrategyAsync();



            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
