using Altinn.Common.Interfaces;
using Altinn2.Adapters.Bindings;
using Altinn2.Adapters.WS.Correspondence;
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
            services.AddScoped<IPrefillClient, PrefillClient>();
            services.AddScoped<IPrefillFormTaskBuilder, PrefillFormTaskBuilder>();
            services.AddScoped<IPrefillAdapter, PrefillAdapter>();

            services.Configure<PrefillConnectionSettings>(configuration.GetSection("PrefillConnectionSettings"));

            return services;
        }

        public static IServiceCollection AddAltinn2CorrespondenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBindingFactory, BindingFactory>();
            services.AddTransient<IBinding, MtomBindingProvider>();
            services.AddTransient<IBinding, BasicBindingProvider>();

            services.AddScoped<ICorrespondenceClient, CorrespondenceClient>();
            services.AddScoped<ICorrespondenceBuilder, CorrespondenceBuilder>();
            
            services.AddScoped<ICorrespondenceAdapter, CorrespondenceAdapter>();            
            services.Configure<CorrespondenceConnectionSettings>(configuration.GetSection("CorrespondenceConnectionSettings"));

            return services;
        }
    }
}
