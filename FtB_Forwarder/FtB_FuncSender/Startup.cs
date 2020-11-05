using Altinn.Common.Interfaces;
using Altinn.Distribution;
using Altinn2.Adapters;
using Altinn3.Adapters;
using FtB_Common.Encryption;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_FormLogic;
using FtB_MessageManager;
using FtB_ProcessStrategies;
using Ftb_Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(FtB_FuncSender.Startup))]
namespace FtB_FuncSender
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


            builder.Services.AddScoped<SendQueueProcessor>();
            builder.Services.AddScoped<IBlobOperations, BlobOperations>();
            builder.Services.AddScoped<BlobStorage>();
            builder.Services.AddLogging();
            

            builder.Services.AddScoped<FormatIdToFormMapper>();
            builder.Services.AddScoped<VarselOppstartPlanarbeidSendLogic>();
            builder.Services.AddScoped<IFormDataRepo, FormDataRepository>();
            builder.Services.AddScoped<ITableStorage, TableStorage>();
            
            builder.Services.AddScoped<IMessageManager, SlackManager>();
            
            builder.Services.AddScoped<IDistributionDataMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>, VarselOppstartPlanarbeidSendDataProvider>();
            builder.Services.AddAltinn2Distribution(configuration);

            builder.Services.AddScoped<VarselOppstartPlanarbeidPrefillMapper>();
            builder.Services.AddScoped<IDecryption, Decryption>();
            builder.Services.AddScoped<IDecryptionFactory, DecryptionFactory>();
            builder.Services.Configure<EncryptionSettings>(configuration.GetSection("EncryptionSettings"));
            builder.Services.AddFtbDbUnitOfWork(configuration);

            //builder.Services.AddAltinn3Distribution(configuration);
            //builder.Services.AddScoped<IDistributionDataMapper<FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType, no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>, VarselOppstartPlanarbeidPrepareAltinn3SendDataProvider>();

            builder.Services.AddAltinnNotification(configuration);
        }
    }
}
