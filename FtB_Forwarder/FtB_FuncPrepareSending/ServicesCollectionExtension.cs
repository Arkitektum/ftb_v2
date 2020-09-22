using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_DistributionDataModels.Forms;
using FtB_PrepareSending;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_FuncPrepareSending
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDistributorPrepareService(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<NaboVarselPlanForm>();
            //services.AddScoped<IForm, NokoAnnaPlanForm>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<PrepareSendingStrategyManager>();
            return services;
        }

        public static IServiceCollection AddDistributorSendService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IForm, NaboVarselPlanSvarForm>();
            return services;
        }
    }
}
