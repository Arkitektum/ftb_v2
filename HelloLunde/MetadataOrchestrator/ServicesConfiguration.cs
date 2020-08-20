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
            services.AddScoped<IComicProvider, ComicStaticProvider>();
            services.AddScoped<IComicProvider, ComicApiProvider>();
            services.AddHttpClient<XkcdService>();

            services.AddOptions<ProviderSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("MetadataProviderSettings").Bind(settings);
            });

            return services;

        }
    }
}
