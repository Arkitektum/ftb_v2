using AltinnWebServices;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic;
using FtB_MessageManager;
using FtB_ProcessStrategies;
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
            builder.Services.AddScoped<NaboVarselPlanFormLogic>();
            builder.Services.AddScoped<IFormDataRepo, FormDataRepository>();
            builder.Services.AddScoped<ITableStorage, TableStorage>();
            builder.Services.AddSendStrategies();
            builder.Services.AddScoped<IPrefillService, PrefillService>();
            builder.Services.AddScoped<IMessageManager, SlackManager>();
            builder.Services.AddAltinn2PrefillService(configuration);

            builder.Services.AddScoped<IPrefillDataProvider<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>, NabovarselSvarPrefillDataProvider>();
        }
    }
}
