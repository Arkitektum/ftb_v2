using AltinnWebServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_FormDownloader
{
    public static class IServicesCollectionExtension
    {
        public static IServiceCollection AddEnqueuedItemsProcessor(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAltinnWebServices();
            services.AddTransient<EnqueuedItemsProcessor>();
            services.AddTransient<AltinnDownloaderService>();
            services.AddTransient<EnqueuedItemsProcessor>();            

            services.AddOptions<ProcessingSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("ProcessingSettings").Bind(settings);
            });
            services.AddOptions<AltinnServiceOwnerConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("AltinnServiceOwnerConnectionSettings").Bind(settings);
            });
            services.AddOptions<AltinnDownloadQueueConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("AltinnDownloadQueueConnectionSettings").Bind(settings);
            });

            return services;
        }
    }
}
