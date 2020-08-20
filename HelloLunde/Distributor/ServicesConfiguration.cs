using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Distributor
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDistributorService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<SlackClient>();
            services.AddScoped<IDistributor, EmailDistributor>();
            services.AddScoped<IDistributor, SlackDistributor>();

            services.AddOptions<DistributorSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("DistributorSettings").Bind(settings);
            });
            services.AddOptions<EmailDistributorSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("EmailDistributorSettings").Bind(settings);
            });
            services.AddOptions<SlackSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("SlackSettings").Bind(settings);
            });


            return services;
        }
    }
}
