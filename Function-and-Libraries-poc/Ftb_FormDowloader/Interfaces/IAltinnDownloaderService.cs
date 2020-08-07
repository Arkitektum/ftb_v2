using AltinnWebServices.Models;
using System.Collections.Generic;

namespace Ftb_FormDownloader
{
    public interface IAltinnDownloaderService
    {
        List<IArchivedItemMetadata> DownloadMetadata();
    }
}