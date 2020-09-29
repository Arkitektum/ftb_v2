using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSendStrategies(this IServiceCollection services)
        {
            services.AddScoped<SenderStrategyManager>();
            services.AddScoped<DefaultDistributionSendStrategy>();
            services.AddScoped<DefaultNotificationSendStrategy>();
            services.AddScoped<DefaultShipmentSendStrategy>();

            services.AddScoped<Func<StrategyTypeEnum, IStrategy<ReportQueueItem, SendQueueItem>>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case StrategyTypeEnum.Distribution:
                        return serviceProvider.GetService<DefaultDistributionSendStrategy>();
                    case StrategyTypeEnum.Notification:
                        return serviceProvider.GetService<DefaultNotificationSendStrategy>();
                    case StrategyTypeEnum.Shipping:
                        return serviceProvider.GetService<DefaultShipmentSendStrategy>();
                    default:
                        throw new NotImplementedException($"{key} strategy not found");
                }
            });
            return services;
        }

        public static IServiceCollection AddPrepareStrategies(this IServiceCollection services)
        {
            services.AddScoped<PrepareSendingStrategyManager>();
            services.AddScoped<DefaultDistributionPrepareStrategy>();
            services.AddScoped<DefaultNotificationPrepareStrategy>();
            services.AddScoped<DefaultShipmentPrepareStrategy>();

            services.AddScoped<Func<StrategyTypeEnum, IStrategy<List<SendQueueItem>, SubmittalQueueItem>>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case StrategyTypeEnum.Distribution:
                        return serviceProvider.GetService<DefaultDistributionPrepareStrategy>();
                    case StrategyTypeEnum.Notification:
                        return serviceProvider.GetService<DefaultNotificationPrepareStrategy>();
                    case StrategyTypeEnum.Shipping:
                        return serviceProvider.GetService<DefaultShipmentPrepareStrategy>();
                    default:
                        throw new NotImplementedException($"{key} strategy not found");
                }
            });

            return services;
        }

        public static IServiceCollection AddReportStrategies(this IServiceCollection services)
        {
            services.AddScoped<ReporterStrategyManager>();
            services.AddScoped<DefaultDistributionReportStrategy>();
            services.AddScoped<DefaultNotificationReportStrategy>();
            services.AddScoped<DefaultShipmentReportStrategy>();

            services.AddScoped<Func<StrategyTypeEnum, IStrategy<FinishedQueueItem, ReportQueueItem>>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case StrategyTypeEnum.Distribution:
                        return serviceProvider.GetService<DefaultDistributionReportStrategy>();
                    case StrategyTypeEnum.Notification:
                        return serviceProvider.GetService<DefaultNotificationReportStrategy>();
                    case StrategyTypeEnum.Shipping:
                        return serviceProvider.GetService<DefaultShipmentReportStrategy>();
                    default:
                        throw new NotImplementedException($"{key} strategy not found");
                }
            });

            return services;
        }

    }
}
