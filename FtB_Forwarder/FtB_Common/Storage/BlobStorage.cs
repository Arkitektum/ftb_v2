using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public abstract class BlobStorage
    {
        protected BlobServiceClient _blobServiceClient;
        protected IConfiguration _configuration;
        protected BlobContainerClient _containerClient;
        protected string _privateAzureStorageConnectionString;
        protected string _publicAzureStorageConnectionString;

        public BlobContainerClient GetBlobContainerClient(string containerName)
        {
            return _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public System.Uri GetBlobUri()
        {
            return _blobServiceClient.Uri;
        }

        public abstract Task CreateContainerIfNotExistsAsync(string containerName);
        public abstract BlockBlobClient GetBlockBlobClient(string containerName, string blobName);

        public Azure.Pageable<BlobItem> GetBlobContainerItems(string containerName)
        {
            _containerClient = GetBlobContainerClient(containerName);
            return _containerClient.GetBlobs(traits: BlobTraits.Metadata);
        }
    }
}
