using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataOrchestrator
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddMetadataOrchestrator(this IServiceCollection services)
        {

            return services;

        }
    }
}
