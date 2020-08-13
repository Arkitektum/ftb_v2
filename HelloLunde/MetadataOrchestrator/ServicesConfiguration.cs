using MetadataEnqueuer;
using MetadataProvider;
using MetadataProvider.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetadataOrchestrator
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddMetadataOrchestrator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrchestrator, Orchestrator>();
            services.AddScoped<IMetadataProviderFactory, MetadataProviderFactory>();
            services.AddScoped<IMetadataProvider, MetadataStaticProvider>();
            services.AddScoped<IMetadataProvider, MetadataApiProvider>();
            services.AddHttpClient<XkcdService>();

            services.AddScoped<IEnqueuer, Enqueuer>();
            services.AddScoped<IQueueClient, AzureQueueClient>();

            services.AddOptions<ProviderSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("MetadataProviderSettings").Bind(settings);
            });

            services.AddOptions<QueueSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("QueueSettings").Bind(settings);
            });

            return services;

        }
    }
}
