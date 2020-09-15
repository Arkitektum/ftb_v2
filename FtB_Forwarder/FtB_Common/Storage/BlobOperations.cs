using FtB_Common.BusinessModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Storage
{
    public class BlobOperations
    {
        ArchivedItemInformation _archivedItem = new ArchivedItemInformation();
        BlobStorage _blobStorage;

        public BlobOperations(string containerName)
        {
            _blobStorage = new BlobStorage(containerName);
            InitiateObjectFromBlobAsync(containerName).GetAwaiter().GetResult();
        }

        private async Task InitiateObjectFromBlobAsync(string containerName)
        {
            try
            {
                containerName = containerName.ToLower();
                var blobContainerClient = _blobStorage.GetBlobContainerClient();
                string archivedItemInfoSerialized = JsonConvert.SerializeObject(_archivedItem);
                var stream = new MemoryStream();
                foreach (var blobItem in _blobStorage.GetBlobContainerItems())
                {
                    if (blobItem.Name.StartsWith("ArchivedItemInformation"))
                    {
                        var client = blobContainerClient.GetBlobClient(blobItem.Name);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetServiceCodeFromStoredBlob()
        {
            return _archivedItem.ServiceCode;
        }
        public string GetFormatIdFromStoredBlob()
        {
            return _archivedItem.DataFormatID;
        }
        public int GetFormatVersionIdFromStoredBlob()
        {
            return _archivedItem.DataFormatVersionID;
        }
    }
}
