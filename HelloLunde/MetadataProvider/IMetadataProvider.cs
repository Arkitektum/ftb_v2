using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProvider
{
    public interface IMetadataProvider
    {
        Task<IEnumerable<MetadataItem>> GetMetadata();

    }
}
