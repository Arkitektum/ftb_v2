using Altinn.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altinn3.Adapters
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddAltinn3PrefillService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<IPrefillAdapter, PrefillAdapter>();


            //services.AddOptions<AltinnPrefillConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            //{
            //    configuration.GetSection("AltinnPrefillConnectionSettings").Bind(settings);
            //});

            return services;
        }

    }
}
