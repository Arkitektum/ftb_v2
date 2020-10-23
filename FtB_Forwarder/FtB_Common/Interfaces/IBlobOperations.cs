using FtB_Common.Enums;
using System;
using System.Collections.Generic;

namespace FtB_Common.Storage
{
    public interface IBlobOperations
    {
        string GetFormatIdFromStoredBlob(string containerName);
        int GetFormatVersionIdFromStoredBlob(string containerName);
        string GetServiceCodeFromStoredBlob(string containerName);
        string GetFormdata(string archiveReference);
        void AddBytesAsBlob(string containerName, string fileName, byte[] fileBytes, string mimeType, IEnumerable<KeyValuePair<string, string>> metadata = null);
        string GetBlobASStringByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaData);
        byte[] GetBlobAsBytesByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
        IEnumerable<(string attachmentType, string fileName)> GetListOfBlobsWithMetadataType(string archiveReference, IEnumerable<BlobStorageMetadataTypeEnum> blobStorageTypes);
    }
}