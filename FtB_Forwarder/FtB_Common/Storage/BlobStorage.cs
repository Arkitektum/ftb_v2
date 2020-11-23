using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;

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
            if (_containerClient == null)
            {
                _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            return _containerClient;
        }

        public abstract BlockBlobClient GetBlockBlobClient(string containerName, string blobName);

        public Azure.Pageable<BlobItem> GetBlobContainerItems(string containerName)
        {
            _containerClient = GetBlobContainerClient(containerName);
            return _containerClient.GetBlobs(traits: BlobTraits.Metadata);
        }
        /*
        public async Task<CloudBlobContainer> SetUpBlobContainerAsync(string containerName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);

            try
            {
                //TODO: Container permissin set for external access
                BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
                await container.CreateIfNotExistsAsync(requestOptions, null);
                //container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            catch (StorageException e)
            {
                Logger.Fatal($"Error setting up Blob contatiner: {e.Message}, {e.ToString()}");
                throw;
            }

            return container;
        }



        public async Task<CloudBlockBlob> AddFileToContainerAsync(CloudBlobContainer cloudBlobContainer, FileInfo fileInfo, string mimeType)
        {
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileInfo.Name);
            blockBlob.Properties.ContentType = mimeType;
            await blockBlob.UploadFromFileAsync(fileInfo.FullName);
            return blockBlob;
        }

        public async Task<CloudBlockBlob> AddFileToContainerAsync(string folderName, FileInfo fileInfo, string mimeType)
        {
            Task<CloudBlockBlob> cloudBlockBlob = AddFileToContainerAsync(GetContainerFromContainerName(folderName), fileInfo, mimeType);
            cloudBlockBlob.Wait();
            return cloudBlockBlob.Result;
        }


        public async Task<CloudBlockBlob> AddFileAsByteArrayToContainer(string containerName, string fileName, byte[] fileBytes, string mimeType)
        {
            CloudBlobContainer container = GetContainerFromContainerName(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = mimeType;
            await blockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
            return blockBlob;
        }

        public async Task<CloudBlockBlob> AddFileAsStreamToContainer(string containerName, string fileName, FileStream filestream, string mimeType)
        {
            CloudBlobContainer container = GetContainerFromContainerName(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = mimeType;
            await blockBlob.UploadFromStreamAsync(filestream);
            return blockBlob;
        }

        public void GetContainerContent(CloudBlobContainer cloudBlobContainer)
        {
            foreach (CloudBlockBlob blob in cloudBlobContainer.ListBlobs())
            {
                // TODO: Write to serilog
                Logger.Debug("- {0} (type: {1})", blob.Uri, blob.GetType());
            }
        }

        public IEnumerable<IListBlobItem> GetCloudBlockBlobsForContainer(CloudBlobContainer cloudBlobContainer)
        {
            return cloudBlobContainer.ListBlobs();
        }



        public void AddOrUpdateContainerMetadata(CloudBlobContainer cloudBlobContainer, string key, string value)
        {
            cloudBlobContainer.Metadata.Add("test", "test2");
            cloudBlobContainer.SetMetadataAsync().Wait();
        }


        public void AddOrUpdateBlobMetadata(CloudBlockBlob cloudBlockBlob, string key, string value)
        {
            cloudBlockBlob.Metadata.Add("test3", "test4");
            cloudBlockBlob.SetMetadataAsync().Wait();
        }


        public CloudBlobContainer GetContainerFromContainerName(string containerName)
        {
            return _blobClient.GetContainerReference(containerName);
        }

        public void GetAllContainers()
        {
            foreach (CloudBlobContainer container in _blobClient.ListContainers())
            {
                Logger.Debug($"- {container.Uri} name: {container.Name}");
            }
        }


        public async Task DownloadFileFromBlobStorageAsync(CloudBlockBlob cloudBlockBlob, string filePathForUpload)
        {
            await cloudBlockBlob.DownloadToFileAsync(filePathForUpload, FileMode.Create);
        }


        public async Task DeleteContainer(string containerName)
        {
            await DeleteContainer(GetContainerFromContainerName(containerName));
        }

        public async Task DeleteContainer(CloudBlobContainer cloudBlobContainer)
        {
            await cloudBlobContainer.DeleteIfExistsAsync();
        }


        public CloudBlockBlob GetBlockBlobFromBlobName(string containerName, string blobName)
        {
            CloudBlobContainer container = GetContainerFromContainerName(containerName);
            return container.GetBlockBlobReference(blobName);
        }


        public void StreamFileFromBlob(string containerName, string fileName, Stream outStream)
        {
            CloudBlockBlob blob = GetBlockBlobFromBlobName(containerName, fileName);

            blob.DownloadToStream(outStream);
        }

        public void StreamFileFromBlob(CloudBlockBlob blob, Stream outStream)
        {
            blob.DownloadToStream(outStream);
        }


        public long GetBlobLength(string containerName, string fileName)
        {
            return GetBlobLength(GetBlockBlobFromBlobName(containerName, fileName));
        }


        public long GetBlobLength(CloudBlockBlob blob)
        {
            blob.FetchAttributes();
            return blob.Properties.Length;
        }

*/
    }
}
