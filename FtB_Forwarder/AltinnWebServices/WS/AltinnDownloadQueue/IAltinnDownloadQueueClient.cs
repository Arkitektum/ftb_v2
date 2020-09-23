using AltinnWebServices.Models;
using System;
using System.Collections.Generic;

namespace AltinnWebServices.WS.AltinnDownloadQueue
{
    public interface IAltinnDownloadQueueClient : IDisposable
    {
        /// <summary>
        /// Retrieves metadata for enqueued items from the altinn download queue
        /// </summary>
        /// <param name="serviceCode">Service code to get items for</param>
        /// <returns></returns>
        List<IArchivedItemMetadata> GetEnqueuedItems(string serviceCode);

        /// <summary>
        /// Retrieves the archived item from altinn
        /// </summary>
        /// <param name="archiveReference"></param>
        /// <returns></returns>
        AltinnArchivedItem GetArchivedItem(string archiveReference);

        /// <summary>
        /// Purges the item from the download queue
        /// </summary>
        /// <param name="archiveReference"></param>
        void PurgeItemFromQueue(string archiveReference);
    }
}
