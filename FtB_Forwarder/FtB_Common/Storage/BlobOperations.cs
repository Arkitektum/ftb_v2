using Altinn.Common.Models;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BlobOperations> _log;
        private PrivateBlobStorage _privateBlobStorage;
        private PublicBlobStorage _publicBlobStorage;
        private string _containerLeaseId;

        public BlobOperations(ILogger<BlobOperations> logger, PrivateBlobStorage privateBlobStorage, PublicBlobStorage publicBlobStorage)
        {
            _log = logger;
            _privateBlobStorage = privateBlobStorage;
            _publicBlobStorage = publicBlobStorage;
        }

        public async Task<bool> AcquireContainerLease(string containerName, int seconds)
        {
            try
            {
                var containerClient = _privateBlobStorage.GetBlobContainerClient(containerName);

                BlobLeaseClient blobLeaseClient = containerClient.GetBlobLeaseClient();
                var blobLeaseResponse = await blobLeaseClient.AcquireAsync(TimeSpan.FromSeconds(seconds));
                _containerLeaseId = blobLeaseClient.LeaseId;
                var httpStatusCode = blobLeaseResponse.GetRawResponse().Status;
                if (httpStatusCode == 201)
                {
                    _log.LogInformation($"Container {containerName} successfully leased with LeaseId={_containerLeaseId}.");
                    return true;
                }
                _log.LogError($"Container {containerName} failed for leasing.");

                return false;
            }
            catch (RequestFailedException rfx)
            {
                if (rfx.Status == 409)
                {
                    _log.LogInformation(rfx, $"Container {containerName} failed for leasing. HTTp-status: {rfx.Status}. Returning false.");
                    return false;
                }
                _log.LogError(rfx, $"Exception: Container {containerName} failed for leasing. HTTp-status: {rfx.Status}. Throwing exception.");
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Exception: Container {containerName} failed for leasing.");
                throw;
            }
        }

        public async Task<bool> ReleaseContainerLease(string containerName)
        {
            try
            {
                var containerClient = _privateBlobStorage.GetBlobContainerClient(containerName);
                BlobLeaseClient blobLeaseClient = containerClient.GetBlobLeaseClient(_containerLeaseId);
                Response<ReleasedObjectInfo> releaseObjectInfo = await blobLeaseClient.ReleaseAsync();
                var xx = releaseObjectInfo.GetRawResponse().Status;
                if (xx == 200)
                {
                    _log.LogInformation($"LeaseId {_containerLeaseId} for container {containerName} successfully released.");
                    return true;
                }
                _log.LogError($"LeaseId {_containerLeaseId} for container {containerName} failed for releasing.");

                return false;
            }
            catch (Exception ex)
            {
                _log.LogError($"Exception: LeaseId {_containerLeaseId} for container {containerName} failed for releasing. Message: {ex.Message}");
                return false;
            }
        }

        private async Task GetArchivedItemFromBlobAsync(string containerName)
        {
            _log.LogDebug("Retrieving archived item from blob storage: {0}", containerName);
            try
            {
                foreach (var blobItem in _privateBlobStorage.GetBlobContainerItems(containerName)) //Filter on metadata here?
                {
                    var metadataItem = blobItem.Metadata.Where(m => m.Key.Equals("type", StringComparison.OrdinalIgnoreCase)
                            && m.Value.Equals(Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.ArchivedItemInformation), StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (!metadataItem.Equals(default(KeyValuePair<string, string>)))
                    {
                        StringBuilder sb = new StringBuilder();
                        var blobContainerClient = _privateBlobStorage.GetBlobContainerClient(containerName);
                        var client = blobContainerClient.GetBlobClient(blobItem.Name);
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
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetServiceCodeFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                await GetArchivedItemFromBlobAsync(containerName);
            }

            return _archivedItem.ServiceCode;
        }
        public async Task<string> GetFormatIdFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                await GetArchivedItemFromBlobAsync(containerName);
            }
            return _archivedItem.DataFormatID;
        }
        public async Task<int> GetFormatVersionIdFromStoredBlob(string containerName)
        {
            if (_archivedItem == null)
            {
                await GetArchivedItemFromBlobAsync(containerName);
            }
            return _archivedItem.DataFormatVersionID;
        }

        public async Task<string> GetFormdata(string containerName)
        {
            try
            {
                foreach (var blobItem in _privateBlobStorage.GetBlobContainerItems(containerName))
                {

                    var metadataItem = blobItem.Metadata.Where(m => (m.Key.Equals("Type", StringComparison.OrdinalIgnoreCase)
                        && m.Value.Equals(Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.FormData), StringComparison.OrdinalIgnoreCase)))
                        .FirstOrDefault();

                    if (!metadataItem.Equals(default(KeyValuePair<string, string>)))
                    {
                        var blobContainerClient = _privateBlobStorage.GetBlobContainerClient(containerName);
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
                        return sb.ToString();
                    }
                }
                throw new ArgumentException($"Formdata i container {containerName} finnes ikke i BlobStorage");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddByteStreamToBlobStorage(BlobStorageEnum storageEnum, string containerName, string identifier, byte[] fileBytes, string mimeType, IEnumerable<KeyValuePair<string, string>> metadata = null)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in metadata)
                dict.Add(item.Key, item.Value);

            BlobStorage storage = GetBlobStorage(storageEnum);

            var client = storage.GetBlockBlobClient(containerName, identifier);
            using (var stream = new MemoryStream(fileBytes, false))
            {
                client.Upload(stream);
            }

            client.SetMetadata(dict);
        }
        private BlobStorage GetBlobStorage(BlobStorageEnum storageEnum)
        {
            return storageEnum == BlobStorageEnum.Private ? (BlobStorage)_privateBlobStorage : (BlobStorage)_publicBlobStorage;
        }
        public string GetBlobASStringByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            var blobItems = _privateBlobStorage.GetBlobContainerItems(containerName);
            var data = string.Empty;
            foreach (var blobItem in blobItems)
            {
                var blob = _privateBlobStorage.GetBlockBlobClient(containerName, blobItem.Name);
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

        public IEnumerable<byte[]> GetBlobsAsBytesByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            BlobStorage storage = GetBlobStorage(storageEnum);
            var blobItems = storage.GetBlobContainerItems(containerName);
            List<byte[]> blobs = new List<byte[]>();
            foreach (var blobItem in blobItems)
            {
                var blobBlock = storage.GetBlockBlobClient(containerName, blobItem.Name);
                BlobProperties properties = blobBlock.GetPropertiesAsync().GetAwaiter().GetResult();
                var matchingMetadataElements = properties.Metadata?.Where(m => metaDataFilter.All(f => m.Key == f.Key && m.Value == f.Value)).ToList();
                foreach (var metadataElement in matchingMetadataElements)
                {
                    var response = blobBlock.Download();
                    var contentLenght = response.Value.ContentLength;
                    using (var binReader = new BinaryReader(response.Value.Content))
                    {
                        var filecontent = binReader.ReadBytes(Convert.ToInt32(contentLenght));
                        blobs.Add(filecontent);
                    }
                }
            }

            return blobs;
        }

        public async Task<IEnumerable<(string attachmentFileName, string attachmentFileUrl, string attachmentType)>> GetBlobUrlsFromPublicStorageByMetadata(string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            var blobItems = _publicBlobStorage.GetBlobContainerItems(containerName);
            List<(string attachmentFileName, string attachmentFileUrl, string attachmentType)> blobUrls = new List<(string attachmentFileName, string attachmentFileUrl, string attachmentType)>();

            foreach (var blobItem in blobItems)
            {
                var blobBlock = _publicBlobStorage.GetBlockBlobClient(containerName, blobItem.Name);
                BlobProperties properties = await blobBlock.GetPropertiesAsync();
                var matchingMetadataElements = properties.Metadata?.Where(m => metaDataFilter.Any(f => m.Key == f.Key && m.Value == f.Value)).ToList();
                foreach (var metadataElement in matchingMetadataElements)
                {
                    blobUrls.Add((blobItem.Name, blobBlock.Uri.AbsoluteUri, metadataElement.Value));
                }
            }
            var blobUrlsSorted = blobUrls.OrderBy(x => x.attachmentType).Select(x => x);

            return blobUrlsSorted;
        }


        public string GetPublicBlobContainerName(string containerName)
        {
            var blobItems = _privateBlobStorage.GetBlobContainerItems(containerName);
            var blobUrls = new List<KeyValuePair<string, string>>();
            string publicBlobContainer = "";

            foreach (var blobItem in blobItems)
            {
                List<string> publicBlobContainerList = blobItem.Metadata.Where(x => x.Key.Equals("PublicBlobContainerName", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();
                if (publicBlobContainerList.Count == 1)
                {
                    publicBlobContainer = publicBlobContainerList[0];
                    break;
                }
            }

            return publicBlobContainer;
        }

        public async Task<IEnumerable<(string attachmentType, string fileName)>> GetListOfBlobsWithMetadataType(BlobStorageEnum storageEnum, string containerName, IEnumerable<BlobStorageMetadataTypeEnum> blobItemTypes)
        {
            BlobStorage storage = GetBlobStorage(storageEnum);
            var listOfAttachments = new List<(string attachmentType, string fileName)>();
            var blobItems = storage.GetBlobContainerItems(containerName);
            foreach (var blobItem in blobItems)
            {
                var blob = storage.GetBlockBlobClient(containerName, blobItem.Name);
                BlobProperties properties = await blob.GetPropertiesAsync();
                var metadataList = properties.Metadata;
                var blobItemTypesAsString = blobItemTypes.Select(x => Enum.GetName(typeof(BlobStorageMetadataTypeEnum), x)).ToList();
                var blobIsOfRequestedType = metadataList.Any(x => x.Key.Equals("Type", StringComparison.OrdinalIgnoreCase) && blobItemTypesAsString.Contains(x.Value));
                var attachment = metadataList.Where(x => x.Key.Equals("AttachmentTypeName", StringComparison.OrdinalIgnoreCase)).Select(x => (attachmentType: x.Value, fileName: blobItem.Name));
                listOfAttachments.AddRange(attachment);
            }

            return listOfAttachments;
        }

        public IEnumerable<Attachment> GetAttachmentsByMetadata(BlobStorageEnum storageEnum, string containerName, IEnumerable<KeyValuePair<string, string>> metaDataFilter)
        {
            BlobStorage storage = GetBlobStorage(storageEnum);
            var blobItems = storage.GetBlobContainerItems(containerName);
            List<Attachment> attachments = new List<Attachment>();
            foreach (var blobItem in blobItems)
            {
                var blobBlock = storage.GetBlockBlobClient(containerName, blobItem.Name);
                BlobProperties properties = blobBlock.GetPropertiesAsync().GetAwaiter().GetResult();
                var blobIsPDF = (properties.ContentType != null && properties.ContentType.ToLower().Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                                    || blobItem.Name.ToLower().Contains(".pdf");
                var blobIsXML = (properties.ContentType != null && properties.ContentType.ToLower().Equals("application/xml", StringComparison.OrdinalIgnoreCase))
                                    || blobItem.Name.ToLower().Contains(".xml");
                var blobIsJson = (properties.ContentType != null && properties.ContentType.ToLower().Equals("application/json", StringComparison.OrdinalIgnoreCase))
                                    || blobItem.Name.ToLower().Contains(".json");

                var matchingMetadataElements = properties.Metadata?.Where(meta => metaDataFilter.ToList()
                                    .Any(filter => filter.Key == meta.Key && filter.Value.Equals(meta.Value, StringComparison.OrdinalIgnoreCase)));

                if (matchingMetadataElements?.Count() > 0)
                {
                    var response = blobBlock.Download();

                    if (blobIsPDF)
                    {
                        var attachment = new AttachmentBinary();
                        attachment = (AttachmentBinary)EnrichTheAttachment(attachment, containerName, blobItem, properties);

                        var contentLenght = response.Value.ContentLength;
                        using (var binReader = new BinaryReader(response.Value.Content))
                        {
                            var filecontent = binReader.ReadBytes(Convert.ToInt32(contentLenght));
                            attachment.BinaryContent = filecontent;
                            attachments.Add(attachment);
                        }
                    }
                    else if (blobIsXML)
                    {
                        var attachment = new AttachmentXml();
                        attachment = (AttachmentXml)EnrichTheAttachment(attachment, containerName, blobItem, properties);
                        var data = string.Empty;
                        using (var reader = new StreamReader(response.Value.Content))
                        {
                            data = reader.ReadToEnd();
                            attachment.XmlStringContent = data;
                            attachments.Add(attachment);
                        }
                    }
                    else if (blobIsJson)
                    {
                        var attachment = new AttachmentJson();
                        attachment = (AttachmentJson)EnrichTheAttachment(attachment, containerName, blobItem, properties);
                        var data = string.Empty;
                        using (var reader = new StreamReader(response.Value.Content))
                        {
                            data = reader.ReadToEnd();
                            attachment.JsonStringContent = data;
                            attachments.Add(attachment);
                        }
                    }
                }
            }

            return attachments;
        }

        private Attachment EnrichTheAttachment(Attachment attachment, string containerName, BlobItem blobItem, BlobProperties properties)
        {
            attachment.ArchiveReference = containerName.ToUpper();
            attachment.AttachmentTypeName = properties.Metadata?.FirstOrDefault(x => x.Key.Equals("AttachmentTypeName", StringComparison.OrdinalIgnoreCase)).Value;
            attachment.Filename = blobItem.Name;
            attachment.Name = properties.Metadata?.FirstOrDefault(x => x.Key.Equals("AttachmentTypeName", StringComparison.OrdinalIgnoreCase)).Value;
            attachment.Type = properties.ContentType;

            return attachment;
        }
    }
}
