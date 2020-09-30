using AltinnWebServices.Bindings;
using AltinnWebServices.Services;
using AltinnWebServices.WS.Prefill;
using FtB_Common.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AltinnWebServices
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddAltinn2PrefillService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBindingFactory, BindingFactory>();
            services.AddTransient<IBinding, MtomBindingProvider>();
            services.AddTransient<IBinding, BasicBindingProvider>();
            services.AddScoped<IAltinnPrefillClient, AltinnPrefillClient>();
            services.AddScoped<IPrefillFormTaskBuilder, PrefillFormTaskBuilder>();
            services.AddScoped<IPrefillAdapter, PrefillAdapter>();            

            services.AddOptions<AltinnPrefillConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("AltinnPrefillConnectionSettings").Bind(settings);
            });

            return services;
        }

    }
}
