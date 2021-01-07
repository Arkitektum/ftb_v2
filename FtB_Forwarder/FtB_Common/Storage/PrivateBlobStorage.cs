using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs.Models;
using FtB_Common.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public class PrivateBlobStorage : BlobStorage
    {
        public PrivateBlobStorage(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["PrivateAzureStorageConnectionString"]);
            _configuration = configuration;
        }
        public override BlockBlobClient GetBlockBlobClient(string containerName, string blobName)
        {
            return new BlockBlobClient(_configuration["PrivateAzureStorageConnectionString"], containerName, blobName);
        }

        public override async Task CreateContainerIfNotExistsAsync(string containerName)
        {
            _containerClient = GetBlobContainerClient(containerName);
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        }
    }
}
