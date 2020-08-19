using MetadataEnqueuer;
using MetadataProvider;
using MetadataProvider.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
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

        public async Task EnqueueMetadata(string addToQueueMessage)
        {
            var result = await _metadataProviderFactory.GetProvider().GetMetadata();
            List<MetadataItem> metadataItems = new List<MetadataItem>();
            foreach (var comicItem in result)
            {
                MetadataItem item = new MetadataItem() { emailTo = addToQueueMessage, comicItem = comicItem };
                metadataItems.Add(item);
            }

            var listOfTasks = new List<Task>();
            foreach (var item in metadataItems)
                listOfTasks.Add(_enqueuer.Enqueue(JsonSerializer.Serialize(item)));

            await Task.WhenAll(listOfTasks);
        }
    }
}
