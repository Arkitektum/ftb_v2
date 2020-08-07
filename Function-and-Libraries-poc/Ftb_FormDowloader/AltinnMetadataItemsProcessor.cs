using AltinnWebServices.Models;
using Ftb_QueueRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace Ftb_FormDownloader
{
    public class AltinnMetadataItemsProcessor : IEnqueuedItemsProcessor
    {
        private readonly ILogger<AltinnMetadataItemsProcessor> _logger;
        private readonly IAltinnDownloaderService _altinnDownloaderService;
        private readonly IQueueClient _queueClient;

        private List<IArchivedItemMetadata> _downloadedMetadataItems;

        public AltinnMetadataItemsProcessor(IAltinnDownloaderService altinnDownloaderService
                                          , IQueueClient queueClient
                                          , ILogger<AltinnMetadataItemsProcessor> logger)
        {
            _logger = logger;
            _altinnDownloaderService = altinnDownloaderService;
            _queueClient = queueClient;
        }

        public async Task EnqueueMetadataFromAltinnDownloadQueue()
        {
            DownloadEnqueuedItems();
            await AddToMetaDataQueue();
        }

        private void DownloadEnqueuedItems()
        {
            _downloadedMetadataItems = _altinnDownloaderService.DownloadMetadata();
        }

        private async Task AddToMetaDataQueue()
        {
            foreach (var item in _downloadedMetadataItems)
            {
                _logger.LogInformation("AddToMetaDataQueue {Reference}", item.ArchiveReference);
                await _queueClient.AddToQueue(new MetadataQueueItem() { Reference = item.ArchiveReference, ServiceCode = item.ServiceCode });
            }
        }
    }
}
