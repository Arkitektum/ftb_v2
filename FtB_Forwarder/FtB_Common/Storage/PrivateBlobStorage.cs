using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using FtB_Common.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
