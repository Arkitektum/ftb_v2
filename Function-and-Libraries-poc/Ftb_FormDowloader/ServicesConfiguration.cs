using AltinnWebServices;
using Ftb_QueueRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_FormDownloader
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddEnqueuedItemsProcessor(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAltinnWebServices();

            services.AddScoped<IEnqueuedItemsProcessor, AltinnMetadataItemsProcessor>();
            services.AddScoped<IQueueClient, QueueClient>();
            services.AddScoped<IAltinnDownloaderService, AltinnDownloaderService>();
            services.AddSingleton<IServiceBusQueueClientFactory, ServiceBusQueueClientFactory>();            
            services.AddSingleton<ServiceBusQueueClientFactory>();

            // Configures options
            services.AddOptions<MessageProcessingSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("MessageProcessingSettings").Bind(settings);
            });
            services.AddOptions<ServiceBusSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("ServiceBusSettings").Bind(settings);
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
