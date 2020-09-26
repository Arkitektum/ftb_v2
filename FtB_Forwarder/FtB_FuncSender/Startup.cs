using FtB_Common.Storage;
using FtB_ProcessStrategies;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FtB_FuncSender.Startup))]
namespace FtB_FuncSender
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<SendQueueProcessor>();
            builder.Services.AddDistributorSendService();
            builder.Services.AddScoped<IBlobOperations, BlobOperations>();
            builder.Services.AddScoped<BlobStorage>();
            //builder.Services.AddScoped<IBlobOperations>();
        }
    }
}
