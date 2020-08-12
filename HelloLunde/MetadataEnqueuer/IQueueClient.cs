using System.Threading.Tasks;

namespace MetadataEnqueuer
{
    public interface IQueueClient
    {
        Task EnqueueMessage(string messageAsJson);
    }
}
