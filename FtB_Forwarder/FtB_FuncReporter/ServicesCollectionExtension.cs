using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_DistributionFormLogic.FormLogic;
using FtB_Reporter;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_FuncReporter
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtBObjects(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<NaboVarselPlanFormLogic>();
            //services.AddScoped<IForm, NokoAnnaPlanForm>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            services.AddScoped<ReporterStrategyManager>();
            return services;
        }
    }
}
