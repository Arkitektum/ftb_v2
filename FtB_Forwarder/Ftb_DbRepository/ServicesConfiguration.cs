using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_Repositories
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtbDbUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHttpClient<FormMetadataHttpClient>();
            //services.AddHttpClient<DistributionFormsHttpClient>();
            //services.AddHttpClient<LogEntryHttpClient>();

            services.AddHttpClient();
            services.AddScoped<FormMetadataHttpClient>();
            services.AddScoped<DistributionFormsHttpClient>();
            services.AddScoped<LogEntryHttpClient>();
            services.AddScoped<DbUnitOfWork>();
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();
            services.AddScoped<IDistributionFormRepository, DistributionFormsRepository>();
            services.AddScoped<IFormMetadataRepository, FormMetadataRepository>();
            services.Configure<FormProcessAPISettings>(configuration.GetSection("FormProcessAPISettings"));
            return services;
        }
    }
}
