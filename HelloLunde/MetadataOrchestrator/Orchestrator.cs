using MetadataEnqueuer;
using MetadataProvider;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetadataOrchestrator
{
    public class Orchestrator
    {
        private readonly IMetadataProvider _metadataProvider;
        private readonly IEnqueuer _enqueuer;

        public Orchestrator(IMetadataProvider metadataProvider, IEnqueuer enqueuer)
        {
            _metadataProvider = metadataProvider;
            _enqueuer = enqueuer;
        }

        public async Task EnqueueMetadata()
        {
            var result = await _metadataProvider.GetMetadata();

            foreach (var item in result)
            {
                await _enqueuer.Enqueue(JsonSerializer.Serialize(item));
            }
        }
    }
}
