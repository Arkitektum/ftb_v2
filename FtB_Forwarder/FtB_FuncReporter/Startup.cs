using Altinn.Distribution;
using FtB_Common.Encryption;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using FtB_FormLogic;
using FtB_MessageManager;
using FtB_ProcessStrategies;
using Ftb_Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(FtB_FuncReporter.Startup))]
namespace FtB_FuncReporter
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var options = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(options.AppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddScoped<ReportQueueProcessor>();
            builder.Services.AddScoped<IBlobOperations, BlobOperations>();
            builder.Services.AddScoped<PrivateBlobStorage>();
            builder.Services.AddScoped<PublicBlobStorage>();
            builder.Services.AddScoped<IHtmlUtils, HtmlUtils>();
            builder.Services.AddScoped<FormatIdToFormMapper>();
            builder.Services.AddScoped<VarselOppstartPlanarbeidReportLogic>();
            builder.Services.AddScoped<SvarVarselOppstartPlanarbeidReportLogic>();
            builder.Services.AddScoped<IFormDataRepo, FormDataRepository>();
            builder.Services.AddScoped<ITableStorage, TableStorage>();
            builder.Services.AddAltinnNotification(configuration);
            builder.Services.AddFtbRepositories(configuration);
            builder.Services.Configure<HtmlUtilSettings>(configuration.GetSection("HtmlUtilSettings"));

            builder.Services.AddLogging();
        }
    }
}
