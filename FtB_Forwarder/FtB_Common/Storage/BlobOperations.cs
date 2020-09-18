using Azure.Storage.Blobs.Models;
using FtB_Common.BusinessModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public class BlobOperations : IBlobOperations
    {
        private ArchivedItemInformation _archivedItem;
        private BlobStorage _blobStorage;
        public BlobOperations(BlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        private void GetArchivedItem(string containerName)
        {
            GetArchivedItemFromBlobAsync(containerName).GetAwaiter().GetResult();
        }
        private async Task GetArchivedItemFromBlobAsync(string containerName)
        {
            try
            {
                var stream = new MemoryStream();
                foreach (var blobItem in _blobStorage.GetBlobContainerItems(containerName))
                {
                    var blobContainerClient = _blobStorage.GetBlobContainerClient(containerName);
                    var client = blobContainerClient.GetBlobClient(blobItem.Name);
                    BlobProperties properties = await client.GetPropertiesAsync();
                    foreach (var metadataItem in properties.Metadata)
                    {
                        if (metadataItem.Key.Equals("Type") && metadataItem.Value.Equals("ArchivedItemInformation"))
                        {
                            StringBuilder sb = new StringBuilder();
                            if (await client.ExistsAsync())
                            {
                                var response = await client.DownloadAsync();
                                using (var streamReader = new StreamReader(response.Value.Content))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        sb.Append(await streamReader.ReadLineAsync());
                                    }
                                }

                            }
                            _archivedItem = JsonConvert.DeserializeObject<ArchivedItemInformation>(sb.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetServiceCodeFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                GetArchivedItemFromBlobAsync(containerName).GetAwaiter().GetResult();
            }
            return _archivedItem.ServiceCode;
        }
        public string GetFormatIdFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                GetArchivedItemFromBlobAsync(containerName).GetAwaiter().GetResult();
            }
            return _archivedItem.DataFormatID;
        }
        public int GetFormatVersionIdFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                GetArchivedItemFromBlobAsync(containerName).GetAwaiter().GetResult();
            }
            return _archivedItem.DataFormatVersionID;
        }

        public string GetFormdata(string containerName)
        {
            try
            {
                foreach (var blobItem in _blobStorage.GetBlobContainerItems(containerName))
                {
                    var blobContainerClient = _blobStorage.GetBlobContainerClient(containerName);
                    var client = blobContainerClient.GetBlobClient(blobItem.Name);
                    BlobProperties properties = client.GetPropertiesAsync().GetAwaiter().GetResult();
                    foreach (var metadataItem in properties.Metadata)
                    {
                        if (metadataItem.Key.Equals("Type") && metadataItem.Value.Equals("FormData"))
                        {
                            StringBuilder sb = new StringBuilder();
                            if (client.ExistsAsync().GetAwaiter().GetResult())
                            {
                                var response = client.DownloadAsync().GetAwaiter().GetResult();
                                using (var streamReader = new StreamReader(response.Value.Content))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        sb.Append(streamReader.ReadLineAsync().GetAwaiter().GetResult());
                                    }
                                }

                            }
                            return sb.ToString();
                        }
                    }
                }
                throw new ArgumentException($"Formdata i container {containerName} finnes ikke i BlobStorage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
