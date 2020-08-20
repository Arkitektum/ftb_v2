using MetadataProvider.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataOrchestrator
{
    public interface IOrchestrator
    {
        Task<IEnumerable<MetadataItem>> EnqueueMetadata(string addToQueueMessage);
        
    }
}