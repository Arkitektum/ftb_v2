using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
