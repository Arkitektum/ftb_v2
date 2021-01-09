using Altinn.Common.Models;
using FtB_Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public interface IBlobOperations
    {
        Task<string> GetFormatIdFromStoredBlob(string containerName);
        Task<int> GetFormatVersionIdFromStoredBlob(string containerName);
        Task<string> GetServiceCodeFromStoredBlob(string containerName);
        Task<string> GetFormdata(string archiveReference);
        Task<string> AddByteStreamToBlobStorage(BlobStorageEnum storageEnum, string containerName, string blobName, byte[] fileBytes, string mimeType, IEnumerable<KeyValuePair<string, string>> metadata = null);
        string GetBlobASStringByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaData);
        Task<byte[]> GetBlobAsBytesByMetadata(BlobStorageEnum storageEnum, string containerName, KeyValuePair<string, string> metaDataFilter);
        IEnumerable<byte[]> GetBlobsAsBytesByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
        IEnumerable<Attachment> GetAttachmentsByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
        Task<IEnumerable<(string attachmentType, string fileName)>> GetListOfBlobsWithMetadataType(BlobStorageEnum storageEnum, string archiveReference, IEnumerable<BlobStorageMetadataTypeEnum> blobStorageTypes);
        Task<IEnumerable<(string attachmentFileName, string attachmentFileUrl, string attachmentType)>> GetBlobUrlsFromPublicStorageByMetadataAsync(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
        string GetPublicBlobContainerName(string containerName);
        Task<bool> AcquireContainerLease(string containerName, int seconds);
        Task<bool> ReleaseContainerLease(string containerName);
        string GetBlobUri(BlobStorageEnum blobStorageEnum);
        Task<BlobContent> GetBlobContentAsBytesByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
        IEnumerable<BlobContent> GetBlobContentsAsBytesByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter);
    }
}