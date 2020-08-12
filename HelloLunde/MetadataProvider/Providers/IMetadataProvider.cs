using MetadataProvider.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    public interface IMetadataProvider
    {
        Task<IEnumerable<MetadataItem>> GetMetadata();

    }
}
