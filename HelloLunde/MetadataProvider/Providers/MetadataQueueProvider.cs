using MetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    [ProviderType(Id = "QueueProvider")]
    public class MetadataQueueProvider : IMetadataProvider
    {
        public Task<IEnumerable<MetadataItem>> GetMetadata()
        {
            throw new NotImplementedException();
        }
    }
}
