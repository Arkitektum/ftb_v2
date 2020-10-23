﻿using Altinn.Common.Models;
using Azure.Storage.Blobs.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                        if (metadataItem.Key.Equals("Type") && metadataItem.Value.Equals(Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.FormData)))
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

        

        public void AddBytesAsBlob(string containerName, string identifier, byte[] fileBytes, string mimeType, IEnumerable<KeyValuePair<string, string>> metadata = null)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in metadata)
                dict.Add(item.Key, item.Value);

            var client = _blobStorage.GetBlockBlobContainerClient(containerName, identifier);

            using (var stream = new MemoryStream(fileBytes, false))
            {
                client.Upload(stream);
            }

            client.SetMetadata(dict);
        }

        public string GetBlobASStringByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            var blobItems = _blobStorage.GetBlobContainerItems(containerName);
            var data = string.Empty;
            foreach (var blobItem in blobItems)
            {
                var blob = _blobStorage.GetBlockBlobContainerClient(containerName, blobItem.Name);
                BlobProperties properties = blob.GetPropertiesAsync().GetAwaiter().GetResult();
                var t = properties.Metadata?.Where(m => metaDataFilter.All(f => m.Key == f.Key && m.Value == f.Value)).ToList();
                if (t?.Count() == metaDataFilter.Count())
                {
                    var response = blob.Download();
                    
                    using (var reader = new StreamReader(response.Value.Content))
                    {   
                        data = reader.ReadToEnd();
                    }
                    break;
                }
            }
            return data;
        }

        public byte[] GetBlobAsBytesByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            var blobItems = _blobStorage.GetBlobContainerItems(containerName);
            byte[] filecontent = Encoding.ASCII.GetBytes("");
            foreach (var blobItem in blobItems)
            {
                var blob = _blobStorage.GetBlockBlobContainerClient(containerName, blobItem.Name);
                BlobProperties properties = blob.GetPropertiesAsync().GetAwaiter().GetResult();
                var t = properties.Metadata?.Where(m => metaDataFilter.All(f => m.Key == f.Key && m.Value == f.Value)).ToList();
                if (t?.Count() == metaDataFilter.Count())
                {
                    var response = blob.Download();
                    var contentLenght = response.Value.ContentLength;
                    using (var binReader = new BinaryReader(response.Value.Content))
                    {
                        filecontent = binReader.ReadBytes(Convert.ToInt32(contentLenght));
                    }
                    break;
                }
            }
            return filecontent;
        }

        public IEnumerable<(string attachmentType, string fileName)> GetListOfBlobsWithMetadataType(string containerName, IEnumerable<BlobStorageMetadataTypeEnum> blobItemTypes)
        {
            var listOfAttachments = new List<(string attachmentType, string fileName)>();
            var blobItems = _blobStorage.GetBlobContainerItems(containerName);
            
            foreach (var blobItem in blobItems)
            {
                var blob = _blobStorage.GetBlockBlobContainerClient(containerName, blobItem.Name);
                BlobProperties properties = blob.GetPropertiesAsync().GetAwaiter().GetResult();
                var metadataList = properties.Metadata;
                var blobItemTypesAsString = blobItemTypes.Select(x => Enum.GetName(typeof(BlobStorageMetadataTypeEnum), x)).ToList();
                var blobIsOfRequestedType = metadataList.Any(x => x.Key.Equals("Type") && blobItemTypesAsString.Contains(x.Value));
                var attachment = metadataList.Where(x => x.Key.Equals("AttachmentTypeName")).Select(x => (attachmentType: x.Value, fileName: blobItem.Name));
                listOfAttachments.AddRange(attachment);
            }

            return listOfAttachments;
        }

    }
}
