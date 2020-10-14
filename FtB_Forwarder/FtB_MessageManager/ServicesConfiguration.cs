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

            services.Configure<EmailManagerSettings>(configuration.GetSection("EmailManagerSettings"));
            services.Configure<SlackManagerSettings>(configuration.GetSection("SlackManagerSettings"));

            return services;
        }
    }
}
