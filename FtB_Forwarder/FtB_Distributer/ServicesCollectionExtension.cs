using FtB_Common.Mappers;
using FtB_DistributionForwarding.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_Distributer
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDistributorPrepareService(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<NaboVarselPlanForm>();
            //services.AddScoped<IForm, NaboVarselForm>();
            //services.AddScoped<IForm, NokoAnnaPlanForm>();
            return services;
        }

        public static IServiceCollection AddDistributorSendService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IForm, NaboVarselPlanSvarForm>();
            return services;
        }
    }
}
