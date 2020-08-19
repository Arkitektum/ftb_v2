using MetadataProvider.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    public interface IComicProvider
    {
        Task<IEnumerable<ComicItem>> GetMetadata();

    }
}
