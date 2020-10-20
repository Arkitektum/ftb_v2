using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_DbRepository
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtbDbUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<DbUnitOfWork>();
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();
            return services;
        }
    }
}
