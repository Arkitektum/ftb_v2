using FtB_Common.BusinessModels;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic;
using FtB_ProcessStrategies;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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
            services.AddReportStrategies();

            //Test reporter
            services.AddScoped<IStrategy<FinishedQueueItem, ReportQueueItem>, TestReportStrategy>();

            return services;
        }
    }
}
