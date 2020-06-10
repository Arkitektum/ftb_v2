using FtB_PreProsessor.CronJobs.Base;
using FtB_PreProsessor.InboundQueue;
using FtB_PreProsessor.OutboundQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace FtB_PreProsessor
{
    class Program
    {
        static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", true, true);
                    configHost.AddCommandLine(args);

                })
            .ConfigureAppConfiguration(configApp =>
            {
                configApp.SetBasePath(Directory.GetCurrentDirectory());
                configApp.AddJsonFile("appsettings.json", true, true);
                configApp.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddCronJob<ScheduledFormProcessor>(c => { c.CronExpression = @"*/30 * * * * *"; c.TimeZoneInfo = TimeZoneInfo.Local; });
                    services.AddScoped<IQueueClient, QueueClient>();
                    services.AddScoped<IInboundQueue, AltinnQueueImpl>();
                    services.AddScoped<IQueueClient, QueueClient>();
                    services.AddScoped<AltinnFormMapper>();
                    services.AddScoped<IFormProcessor, FormProcessor>();
                    services.AddScoped<IServiceBusQueueClientFactory, ServiceBusQueueClientFactory>();
                    services.AddOptions();
                    services.Configure<ScheduledDownloaderConfig>(hostContext.Configuration.GetSection(nameof(ScheduledDownloaderConfig)));
                })
            ;

    }
}
