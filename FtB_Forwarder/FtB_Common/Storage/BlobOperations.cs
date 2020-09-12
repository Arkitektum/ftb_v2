using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Storage
{
    public static class BlobOperations
    {
        public static string GetFormatIdFromStoredBlob(string archiveReference)
        {
            BlobStorage _blobStorage = new BlobStorage(archiveReference);
            string formatID = _blobStorage.GetBlobName(archiveReference);
            return "6325";
        }
    }
}
