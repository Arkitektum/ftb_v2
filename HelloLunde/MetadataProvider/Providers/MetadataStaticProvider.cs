using MetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    [ProviderType(Id = "StaticProvider")]
    public class MetadataStaticProvider : IMetadataProvider
    {
        public Task<IEnumerable<MetadataItem>> GetMetadata()
        {
            return GetStaticContent();
        }

    private async Task<IEnumerable<MetadataItem>> GetStaticContent()
        {
            var result = new List<MetadataItem>
            {
                new MetadataItem() { ArchiveReference = "AR1234", Info = "Integration test 1", ServiceCode = "5678" },
                new MetadataItem() { ArchiveReference = "AR4321", Info = "Integration test 2", ServiceCode = "5678" }
            };
            return result;
        }        
    }
}
