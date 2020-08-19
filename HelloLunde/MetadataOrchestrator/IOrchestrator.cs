using System.Threading.Tasks;

namespace MetadataOrchestrator
{
    public interface IOrchestrator
    {
        Task EnqueueMetadata(string addToQueueMessage);
    }
}