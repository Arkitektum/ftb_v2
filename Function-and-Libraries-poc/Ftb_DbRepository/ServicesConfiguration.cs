using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ftb_DbRepository
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtbDbRepository(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FtbDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IFormMetadataRepository, FormMetadataRepository>();
            return services;
        }
    }
}
