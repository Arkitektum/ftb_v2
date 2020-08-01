namespace Ftb_FormDownloader
{
    public class EnqueuedItemsProcessor
    {
        private readonly AltinnDownloaderService _altinnDownloaderService;

        public EnqueuedItemsProcessor(AltinnDownloaderService altinnDownloaderService)
        {
            _altinnDownloaderService = altinnDownloaderService;
        }

        public void DownloadEnqueuedItems()
        {

            _altinnDownloaderService.DownloadStuff();
        }
    }
}
