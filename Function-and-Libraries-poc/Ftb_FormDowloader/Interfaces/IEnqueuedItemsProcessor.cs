using System.Threading.Tasks;

namespace Ftb_FormDownloader
{
    public interface IEnqueuedItemsProcessor
    {
        Task EnqueueMetadataFromAltinnDownloadQueue();
    }
}