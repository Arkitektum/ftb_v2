using MetadataEnqueuer;
using MetadataProvider;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetadataOrchestrator
{
    public class Orchestrator : IOrchestrator
    {
        private readonly IMetadataProviderFactory _metadataProviderFactory;
        private readonly IEnqueuer _enqueuer;

        public Orchestrator(IMetadataProviderFactory providerFactory, IEnqueuer enqueuer)
        {
            _metadataProviderFactory = providerFactory;
            _enqueuer = enqueuer;
        }

        public async Task EnqueueMetadata()
        {
            var result = await _metadataProviderFactory.GetProvider().GetMetadata();

            var listOfTasks = new List<Task>();
            foreach (var item in result)            
                listOfTasks.Add(_enqueuer.Enqueue(JsonSerializer.Serialize(item)));

            await Task.WhenAll(listOfTasks);
        }
    }
}
