using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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

            return services;
        }
    }
}
