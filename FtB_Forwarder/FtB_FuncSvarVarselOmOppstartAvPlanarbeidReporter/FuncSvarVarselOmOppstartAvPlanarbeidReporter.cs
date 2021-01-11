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
using Ftb_Repositories;

namespace FuncSvarVarselOmOppstartAvPlanarbeidReporter
{
    public class FuncSvarVarselOmOppstartAvPlanarbeidReporter
    {
        private readonly ILogger<FuncSvarVarselOmOppstartAvPlanarbeidReporter> _logger;
        private readonly ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor _reportProcessor;
        private readonly DbUnitOfWork _dbUnitOfWork;

        public FuncSvarVarselOmOppstartAvPlanarbeidReporter(ILogger<FuncSvarVarselOmOppstartAvPlanarbeidReporter> logger, 
                                                            ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor processor,
                                                            DbUnitOfWork dbUnitOfWork)
        {
            _logger = logger;
            _reportProcessor = processor;
            _dbUnitOfWork = dbUnitOfWork;
        }

        [FunctionName("ReportSvarVarselOmOppstartAvPlanarbeid")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var result = await _reportProcessor.ExecuteProcessingStrategyAsync();

                string reportSubmittals = "";
                string separator = "";
                foreach (var submittal in result)
                {
                    reportSubmittals = $"{reportSubmittals}{separator}SubmitterId: {submittal.Item1} ArchiveReference: {submittal.Item2}";
                    separator = ", ";
                }

                string timeStamp = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                string responseMessage = string.IsNullOrEmpty(reportSubmittals)
                    ? "This HTTP triggered function executed successfully, but nothing to report."
                    : $"This HTTP triggered function executed successfully, and the following were reported for: {reportSubmittals}. ";

                await _dbUnitOfWork.SaveLogEntries();

                return new OkObjectResult(timeStamp + ": " + responseMessage);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }

        }
    }
}
