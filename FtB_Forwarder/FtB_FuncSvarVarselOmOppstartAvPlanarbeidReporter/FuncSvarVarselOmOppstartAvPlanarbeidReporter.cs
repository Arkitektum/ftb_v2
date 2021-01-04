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

namespace FuncSvarVarselOmOppstartAvPlanarbeidReporter
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

            var result = await _reportProcessor.ExecuteProcessingStrategyAsync();

            string reportSubmittals = "";
            string separator = "";
            foreach (var submittal in result)
            {
                reportSubmittals = $"{reportSubmittals}{separator}SubmitterId: {submittal.Item1} ArchiveReference: {submittal.Item2}";
                separator = ", ";
            }
            
            string responseMessage = string.IsNullOrEmpty(reportSubmittals)
                ? "This HTTP triggered function executed successfully, but nothing to report."
                : $"This HTTP triggered function executed successfully, and the following were reported for: {reportSubmittals}. ";

            return new OkObjectResult(responseMessage);
        }
    }
}
