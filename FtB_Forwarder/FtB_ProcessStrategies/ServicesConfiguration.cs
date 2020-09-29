using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSendStrategies(this IServiceCollection services)
        {
            services.AddScoped<SenderStrategyManager>();
            services.AddScoped<IStrategy<ReportQueueItem, SendQueueItem>, DefaultDistributionSendStrategy>();
            services.AddScoped<IStrategy<ReportQueueItem, SendQueueItem>, DefaultNotificationSendStrategy>();
            services.AddScoped<IStrategy<ReportQueueItem, SendQueueItem>, DefaultShipmentSendStrategy>();            

            return services;
        }

        public static IServiceCollection AddPrepareStrategies(this IServiceCollection services)
        {
            services.AddScoped<PrepareSendingStrategyManager>();
            services.AddScoped<IStrategy<List<SendQueueItem>, SubmittalQueueItem>, DefaultDistributionPrepareStrategy >();
            services.AddScoped<IStrategy<List<SendQueueItem>, SubmittalQueueItem>, DefaultNotificationPrepareStrategy>();
            services.AddScoped<IStrategy<List<SendQueueItem>, SubmittalQueueItem>, DefaultShipmentPrepareStrategy>();

            return services;
        }

        public static IServiceCollection AddReportStrategies(this IServiceCollection services)
        {
            services.AddScoped<ReporterStrategyManager>();
            services.AddScoped<IStrategy<FinishedQueueItem, ReportQueueItem>, DefaultDistributionReportStrategy>();
            services.AddScoped<IStrategy<FinishedQueueItem, ReportQueueItem>, DefaultDistributionReportStrategy>();
            services.AddScoped<IStrategy<FinishedQueueItem, ReportQueueItem>, DefaultDistributionReportStrategy>();

            return services;
        }

    }
}
