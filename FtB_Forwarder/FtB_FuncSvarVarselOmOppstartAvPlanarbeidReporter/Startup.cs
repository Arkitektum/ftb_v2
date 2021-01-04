using Altinn.Distribution;
using FtB_Common.Encryption;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using FtB_ProcessStrategies;
using Ftb_Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(FtB_FuncAccumulationReporter.Startup))]
namespace FtB_FuncAccumulationReporter
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

            builder.Services.AddLogging();
            builder.Services.AddScoped<IBlobOperations, BlobOperations>();
            builder.Services.AddScoped<PrivateBlobStorage>();
            builder.Services.AddScoped<PublicBlobStorage>();
            builder.Services.AddScoped<IHtmlUtils, HtmlUtils>();
            builder.Services.AddScoped<IFormDataRepo, FormDataRepository>();
            builder.Services.AddScoped<ITableStorage, TableStorage>();
            builder.Services.AddScoped<IDecryption, Decryption>();
            builder.Services.AddScoped<IDecryptionFactory, DecryptionFactory>();
            builder.Services.Configure<EncryptionSettings>(configuration.GetSection("EncryptionSettings"));
            builder.Services.Configure<HtmlUtilSettings>(configuration.GetSection("HtmlUtilSettings"));
            builder.Services.AddScoped<ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor>();
            builder.Services.AddFtbRepositories(configuration);
            builder.Services.AddAltinn2Distribution(configuration);
            builder.Services.AddAltinnNotification(configuration);
        }
    }
}
