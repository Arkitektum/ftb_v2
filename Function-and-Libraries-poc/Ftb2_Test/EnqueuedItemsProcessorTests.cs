using AltinnWebServices.Models;
using Ftb_FormDownloader;
using Ftb_QueueRepository;
using Ftb2_IntegrationTest.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Ftb2_IntegrationTest
{
    public class EnqueuedItemsProcessorTests
    {
        private readonly IQueueClient _queueClient;

        public EnqueuedItemsProcessorTests(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }
        
        [Fact]
        public async Task TestAddToFtBMetadataQueue()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            var downloadServiceMock = new AltinnDownloaderServiceMock();
            var logger = new Logger<AltinnMetadataItemsProcessor>(loggerFactory);
            AltinnMetadataItemsProcessor processor = new AltinnMetadataItemsProcessor(downloadServiceMock, _queueClient, logger);
            await processor.EnqueueMetadataFromAltinnDownloadQueue();

        }
        [Fact]
        public async Task TestAddToFtBMetadataQueueWithMoq()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();

            var metadataItem = new ArchivedItemMetadata(archiveReference: $"AR{new Random().Next(5000, 9999)}"
                                    , archivedDate: DateTime.Now
                                    , reporteeId: ""
                                    , reporteeType: "Person"
                                    , serviceCode: "7777"
                                    , serviceEditionCode: 1);
            var metaDataItemList = new List<IArchivedItemMetadata>() { metadataItem };
            var altinnDownloaderServiceMock = new Mock<IAltinnDownloaderService>();
            altinnDownloaderServiceMock.Setup(x => x.DownloadMetadata()).Returns(metaDataItemList);
            var logger = new Logger<AltinnMetadataItemsProcessor>(loggerFactory);
            AltinnMetadataItemsProcessor processor = new AltinnMetadataItemsProcessor(altinnDownloaderServiceMock.Object, _queueClient, logger);
            await processor.EnqueueMetadataFromAltinnDownloadQueue();

        }
    }
}
