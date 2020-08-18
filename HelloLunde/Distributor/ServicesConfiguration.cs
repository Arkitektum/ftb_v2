using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Distributor
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDistributorService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributor, EmailDistributor>();
            services.AddOptions<DistributorSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("DistributorSettings").Bind(settings);
            }); 
            services.AddOptions<ServiceBusSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("ServiceBusSettings").Bind(settings);
            });

            return services;
        }
    }
}
