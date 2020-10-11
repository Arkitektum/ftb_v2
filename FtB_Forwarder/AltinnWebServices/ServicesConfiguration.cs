using Altinn.Common.Interfaces;
using Altinn2.Adapters.Adapters;
using Altinn2.Adapters.Bindings;
using Altinn2.Adapters.WS.Prefill;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altinn2.Adapters
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

        public static IServiceCollection AddAltinn2CorrespondenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBindingFactory, BindingFactory>();
            services.AddTransient<IBinding, MtomBindingProvider>();
            services.AddTransient<IBinding, BasicBindingProvider>();
            //services.AddScoped<IAltinnCorrespondenceClient, Altinn2.Adapters.WS.Correspondence.CorrespondenceClient >();
            //services.AddScoped<ICorrespondenceBuilder, Altinn2.Adapters.WS.Correspondence.CorrespondanceBuilder>();
            services.AddScoped<ICorrespondenceAdapter, CorrespondenceAdapter>();

            services.AddOptions<WS.Correspondence.AltinnCorrespondenceConnectionSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("AltinnCorrespondenceConnectionSettings").Bind(settings);
            });

            return services;
        }
    }
}
