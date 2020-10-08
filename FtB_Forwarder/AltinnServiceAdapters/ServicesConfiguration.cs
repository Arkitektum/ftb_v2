using FtB_Common.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altinn3ServiceAdapters
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddAltinn3PrefillService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<IPrefillAdapter, Altinn3PrefillAdapter>();
            

            //services.AddOptions<AltinnPrefillConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            //{
            //    configuration.GetSection("AltinnPrefillConnectionSettings").Bind(settings);
            //});

            return services;
        }

    }
}
