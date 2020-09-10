using System.Threading.Tasks;

namespace Ftb_QueueRepository
{
    public interface IQueueClient
    {
        Task AddToQueue(IMetadataQueueItem queueItem);
    }
}