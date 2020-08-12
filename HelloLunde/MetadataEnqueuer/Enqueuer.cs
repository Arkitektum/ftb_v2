using System.Threading.Tasks;

namespace MetadataEnqueuer
{
    public class Enqueuer : IEnqueuer
    {
        private readonly IQueueClient _queueClient;

        public Enqueuer(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task Enqueue(string metadataItemAsJson)
        {
            await _queueClient.EnqueueMessage(metadataItemAsJson);
        }
    }
}
