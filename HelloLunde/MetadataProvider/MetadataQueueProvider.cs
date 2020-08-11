using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProvider
{
    public class MetadataQueueProvider : IMetadataProvider
    {
        public Task<IEnumerable<MetadataItem>> GetMetadata()
        {
            throw new NotImplementedException();
        }
    }
}
