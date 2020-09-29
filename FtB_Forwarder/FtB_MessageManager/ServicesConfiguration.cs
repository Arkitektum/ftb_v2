using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_MessageManager
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddMessageManagerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<SlackClient>();
            services.AddScoped<IMessageManager, EmailManager>();
            services.AddScoped<IMessageManager, SlackManager>();

            services.AddOptions<EmailManagerSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("EmailManagerSettings").Bind(settings);
            });
            services.AddOptions<SlackManagerSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("SlackManagerSettings").Bind(settings);
            });

            return services;
        }
    }
}
