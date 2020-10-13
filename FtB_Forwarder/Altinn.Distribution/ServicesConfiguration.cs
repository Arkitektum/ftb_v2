using Altinn.Common.Interfaces;
using Altinn2.Adapters;
using Altinn2.Adapters.Bindings;
using Altinn2.Adapters.WS.Prefill;
using Altinn3.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altinn.Distribution
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddAltinn2Distribution(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAltinn2PrefillService(configuration);
            services.AddAltinn2CorrespondenceService(configuration);
            services.AddScoped<IDistributionAdapter, Altinn2Distribution>();

            return services;
        }

        public static IServiceCollection AddAltinn3Distribution(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAltinn3PrefillService(configuration);
            services.AddAltinn2CorrespondenceService(configuration);
            services.AddScoped<IDistributionAdapter, Altinn3Distribution>();
            return services;
        }

        public static IServiceCollection AddAltinnNotification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAltinn2CorrespondenceService(configuration);
            services.AddScoped<INotificationAdapter, AltinnNotification>();

            return services;
        }

    }
}
