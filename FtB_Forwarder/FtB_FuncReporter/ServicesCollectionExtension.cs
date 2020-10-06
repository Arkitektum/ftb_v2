using FtB_Common.BusinessModels;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic;
using FtB_ProcessStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FtB_FuncReporter
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddFtBObjects(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<NaboVarselPlanFormLogic>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            //services.AddReportStrategies();

            //Test reporter
            //services.AddScoped<TestReportStrategy>();
            //services.AddScoped<Func<StrategyTypeEnum, IStrategy<FinishedQueueItem, ReportQueueItem>>>(serviceProvider => key =>
            //{
            //    if (key == StrategyTypeEnum.Distribution)
            //        return serviceProvider.GetService<TestReportStrategy>();
            //    else return null;
            //});

            return services;
        }
    }
}
