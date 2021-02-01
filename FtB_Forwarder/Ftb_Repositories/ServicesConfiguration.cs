using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_Repositories
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<FormMetadataHttpClient>();
            services.AddHttpClient<DistributionFormsHttpClient>();
            services.AddHttpClient<LogEntryHttpClient>();
            services.AddHttpClient<HtmlToPdfConverterHttpClient>();
            services.AddHttpClient<FileDownloadStatusHttpClient>();
            services.AddScoped<DbUnitOfWork>();
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();
            services.AddScoped<IDistributionFormRepository, DistributionFormsRepository>();
            services.AddScoped<IFormMetadataRepository, FormMetadataRepository>();
            services.Configure<FormProcessAPISettings>(configuration.GetSection("FormProcessAPISettings"));
            services.Configure<HtmlToPdfConverterSettings>(configuration.GetSection("HtmlToPdfConverterSettings"));
            return services;
        }
    }
}
