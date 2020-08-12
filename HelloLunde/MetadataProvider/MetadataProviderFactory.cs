using MetadataProvider.Providers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataProvider
{
    public class MetadataProviderFactory : IMetadataProviderFactory
    {
        private readonly IEnumerable<IMetadataProvider> metadataProviders;
        private readonly IOptions<ProviderSettings> options;

        public MetadataProviderFactory(IEnumerable<IMetadataProvider> metadataProviders, IOptions<ProviderSettings> options)
        {
            this.metadataProviders = metadataProviders;

            if (string.IsNullOrEmpty(options?.Value?.ProviderType))
                throw new ArgumentException("Ensure that MetadataProviderSettings is configured");

            this.options = options;
        }

        public IMetadataProvider GetProvider()
        {
            foreach (var item in metadataProviders)
            {
                var type = item.GetType();
                var attribute = type.GetCustomAttributes(typeof(ProviderTypeAttribute), false).FirstOrDefault() as ProviderTypeAttribute;

                if (attribute.Id.Equals(options.Value.ProviderType))
                    return item;
            }

            return null;
        }
    }
}
