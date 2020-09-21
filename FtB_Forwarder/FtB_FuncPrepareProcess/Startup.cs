using FtB_Common.Storage;
using FtB_FuncPrepareProcess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FtB_FuncPrepareProcess.Startup))]
namespace FtB_FuncPrepareProcess
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ArchivedItemQueueProcessor>();
            builder.Services.AddDistributorPrepareService();
            builder.Services.AddScoped<BlobStorage>();
            //builder.Services.AddScoped<IBlobOperations>();
        }
    }
}
