using FtB_Common.Storage;
using FtB_ProcessStrategies;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(FtB_FuncPrepareSending.Startup))]
namespace FtB_FuncPrepareSending
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<SubmittalQueueProcessor>();
            builder.Services.AddPrepareServices();
            //builder.Services.AddLogging();
        }
    }
}
