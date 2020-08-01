﻿using AltinnWebServices.Bindings;
using Microsoft.Extensions.DependencyInjection;

namespace AltinnWebServices
{
    public static class IServicesCollectionExtension
    {
        public static IServiceCollection AddAltinnWebServices(this IServiceCollection services)
        {
            services.AddTransient<IBindingFactory, BindingFactory>();
            services.AddTransient<IBinding, MtomBindingProvider>();
            return services;
        }
    }
}
