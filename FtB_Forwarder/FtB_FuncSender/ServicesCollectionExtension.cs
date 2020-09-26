using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic;
using FtB_ProcessStrategies;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_FuncSender
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDistributorSendService(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<NaboVarselPlanFormLogic>();
            //services.AddScoped<IForm, NokoAnnaPlanForm>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            services.AddScoped<SenderStrategyManager>();
            return services;
        }
    }
}
