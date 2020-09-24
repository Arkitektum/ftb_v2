﻿using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_DistributionFormLogic.FormLogic;
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
            services.AddScoped<NaboVarselPlanFormLogic>();
            //services.AddScoped<IForm, NokoAnnaPlanForm>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            services.AddScoped<PrepareSendingStrategyManager>();
            return services;
        }
    }
}
