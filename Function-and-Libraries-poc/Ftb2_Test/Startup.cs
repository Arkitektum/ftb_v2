using Ftb_QueueRepository;
using Xunit.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Ftb2_IntegrationTest
{
    public class Startup
    {
        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("appsettings.json", true, true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IQueueClient, QueueClient>();
                services.AddScoped<IServiceBusQueueClientFactory, ServiceBusQueueClientFactory>();
                services.AddOptions();
                services.AddOptions<ServiceBusSettings>().Configure<IConfiguration>((settings, config) =>
                {
                    hostContext.Configuration.GetSection("ServiceBusSettings").Bind(settings);
                });
            })
            ;

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IQueueClient, QueueClient>();
            //services.AddTransient<IServiceBusQueueClientFactory, ServiceBusQueueClientFactory>();
            //services.AddOptions();
        }
    }
}
