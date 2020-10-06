using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_FormLogic;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_FuncPrepareSending
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddPrepareServices(this IServiceCollection services)
        {
            services.AddScoped<FormatIdToFormMapper>();
            //services.AddScoped<NaboVarselPlanFormLogic>();
            services.AddScoped<VarselOppstartPlanarbeidPrepareLogic>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            services.AddScoped<IBlobOperations, BlobOperations>();
            services.AddScoped<BlobStorage>();

            //services.AddPrepareStrategies();

            //Test reporter
            //services.AddScoped<TestPrepareStrategy>();
            //services.AddScoped<Func<StrategyTypeEnum, IStrategy<List<SendQueueItem>, SubmittalQueueItem>>>(serviceProvider => key =>
            //{
            //    if (key == StrategyTypeEnum.Distribution)
            //        return serviceProvider.GetService<TestPrepareStrategy>();
            //    else return null;
            //});

            return services;
        }
    }
}
