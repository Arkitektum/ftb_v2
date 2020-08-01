using AltinnWebServices;
using AltinnWebServices.Bindings;
using AltinnWebServices.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Ftb_FormDownloader
{
    public class AltinnDownloaderService
    {
        private readonly IBindingFactory _bindingFactory;
        private readonly IOptions<AltinnDownloadQueueConnectionSettings> _connectionOptions;
        private readonly IOptions<ProcessingSettings> _processingOptions;

        public AltinnDownloaderService(IBindingFactory bindingFactory, IOptions<AltinnDownloadQueueConnectionSettings> connectionOptions, IOptions<ProcessingSettings> processingOptions)
        {
            _bindingFactory = bindingFactory;
            _connectionOptions = connectionOptions;
            _processingOptions = processingOptions;
        }

        public void DownloadStuff()
        {
            var downloadItems = new List<IArchivedItemMetadata>();

            using(var altinnClient = new AltinnWebServices.WS.AltinnDownloadQueue.AltinnDownloadQueueClient(_bindingFactory.GetBindingFor(BindingType.Mtom), _connectionOptions))
            {
                foreach (var serviceCode in _processingOptions.Value.ServiceCodes)
                {
                    downloadItems.AddRange(altinnClient.GetEnqueuedItems(serviceCode));
                }
            }

            //TODO Persist metadata

            //TODO Purge from download queue if it is decided that it is sufficient that to download only metadata at this point
        }
    }
}
