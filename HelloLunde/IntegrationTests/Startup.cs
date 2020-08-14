using Distributor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IntegrationTests
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
               services.AddScoped<IDistributor, EmailDistributor>();
               services.AddOptions();
               services.AddOptions<DistributorSettings>().Configure<IConfiguration>((settings, config) =>
               {
                   hostContext.Configuration.GetSection("DistributorSettings").Bind(settings);
               });
           })
           ;
    }
}
