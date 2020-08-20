using MetadataProvider;
using MetadataProvider.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataOrchestrator
{
    public class Orchestrator : IOrchestrator
    {
        private readonly IMetadataProviderFactory _metadataProviderFactory;

        public Orchestrator(IMetadataProviderFactory providerFactory)
        {
            _metadataProviderFactory = providerFactory;
        }

        public async Task<IEnumerable<MetadataItem>> RetreiveMetadata(string addToQueueMessage)
        {
            var result = await _metadataProviderFactory.GetProvider().GetMetadata();
            var metadataItems = new List<MetadataItem>();
            foreach (var comicItem in result)
            {
                var item = new MetadataItem() { emailTo = addToQueueMessage, comicItem = comicItem };
                metadataItems.Add(item);
            }

            return metadataItems;
        }
    }
}
