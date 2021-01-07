using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public class PublicBlobStorage : BlobStorage
    {
        public PublicBlobStorage(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["PublicAzureStorageConnectionString"]);
            _configuration = configuration;
        }
        public override BlockBlobClient GetBlockBlobClient(string containerName, string blobName)
        {
            return new BlockBlobClient(_configuration["PublicAzureStorageConnectionString"], containerName, blobName);
        }
        public override async Task CreateContainerIfNotExistsAsync(string containerName)
        {
            _containerClient = GetBlobContainerClient(containerName);
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }
    }
}
