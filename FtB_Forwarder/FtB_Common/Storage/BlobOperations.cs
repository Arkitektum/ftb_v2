using FtB_Common.BusinessModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FtB_Common.Storage
{
    public class BlobOperations
    {
        ArchivedItemInformation _archivedItem = new ArchivedItemInformation();
        BlobStorage _blobStorage;

        public BlobOperations(string containerName)
        {
            _blobStorage = new BlobStorage(containerName);
            InitiateObjectFromBlob(containerName);
        }

        private void InitiateObjectFromBlob(string containerName)
        {
            try
            {
                containerName = containerName.ToLower();
                var blobContainerClient = _blobStorage.GetBlobContainerClient();
                var stream = new MemoryStream();
                System.Xml.Serialization.XmlSerializer _serializer = new System.Xml.Serialization.XmlSerializer(_archivedItem.GetType());

                foreach (var blobItem in _blobStorage.GetBlobContainerItems())
                {
                    if (blobItem.Name.StartsWith("ArchivedItemInformation"))
                    {
                        var client = blobContainerClient.GetBlobClient(blobItem.Name);
                        client.DownloadToAsync(stream);
                        _serializer.Deserialize(stream);
                    }
                }
                throw new ArgumentException($"Error when retrieving service code from BlobStorage (container name {containerName})");
            }
            catch (Exception)
            {
                throw;
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
