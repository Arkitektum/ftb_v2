using AltinnWebServices;
using AltinnWebServices.Bindings;
using AltinnWebServices.Models;
using Ftb_QueueRepository;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Ftb_FormDownloader
{
    public class AltinnDownloaderService : IAltinnDownloaderService
    {
        private readonly IBindingFactory _bindingFactory;
        private readonly IOptions<AltinnDownloadQueueConnectionSettings> _connectionOptions;
        private readonly IOptions<MessageProcessingSettings> _messageProcessingOptions;

        public AltinnDownloaderService(IBindingFactory bindingFactory, IOptions<AltinnDownloadQueueConnectionSettings> connectionOptions
                                    , IOptions<MessageProcessingSettings> messageProcessingOptions)
        {
            _bindingFactory = bindingFactory;
            _connectionOptions = connectionOptions;
            _messageProcessingOptions = messageProcessingOptions;
        }

        public List<IArchivedItemMetadata> DownloadMetadata()
        {
            var downloadItems = new List<IArchivedItemMetadata>();

            using (var altinnClient = new AltinnWebServices.WS.AltinnDownloadQueue.AltinnDownloadQueueClient(_bindingFactory.GetBindingFor(BindingType.Mtom), _connectionOptions))
            {
                foreach (var serviceCode in _messageProcessingOptions.Value.ServiceCodes)
                {
                    downloadItems.AddRange(altinnClient.GetEnqueuedItems(serviceCode));
                }
            }

            return downloadItems;
            //Maybe.....(see below)
            //TODO Persist metadata
            //TODO Purge from download queue if it is decided that it is sufficient that to download only metadata at this point
        }
    }
}
