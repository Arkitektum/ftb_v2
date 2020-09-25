using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_MessageManager
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddMessageManagerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<SlackClient>();
            services.AddScoped<IMessageManager, EmailManager>();
            services.AddScoped<IMessageManager, SlackManager>();

            //services.AddOptions<MessageManagerSettings>().Configure<IConfiguration>((settings, config) =>
            //{
            //    configuration.GetSection("DistributorSettings").Bind(settings);
            //});
            services.AddOptions<EmailManagerSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("EmailDistributorSettings").Bind(settings);
            });
            services.AddOptions<SlackManagerSettings>().Configure<IConfiguration>((settings, config) =>
            {
                configuration.GetSection("SlackDistributorSettings").Bind(settings);
            });


            return services;
        }
    }
}
