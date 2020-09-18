using FtB_Common.Storage;
using FtB_Distributer;
using FtB_InitiateForwarding;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FtB_FuncForwarding.Startup))]
namespace FtB_FuncForwarding
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ArchivedItemQueueProcessor>();
            builder.Services.AddDistributorPrepareService();
            builder.Services.AddScoped<BlobStorage>();
            builder.Services.AddScoped<IBlobOperations>();
        }
    }
}
