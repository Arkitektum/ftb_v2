using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class NotificationPrepareLogic<T> : PrepareLogic<T>
    {
        protected readonly IBlobOperations _blobOperations;

        public NotificationPrepareLogic(IFormDataRepo repo,
                                        ITableStorage tableStorage,
                                        IBlobOperations blobOperations,
                                        ILogger log,
                                        DbUnitOfWork dbUnitOfWork,
                                        IDecryptionFactory decryptionFactory)
            : base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        {
            _blobOperations = blobOperations;
        }
        public override async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            var returnValue = await base.ExecuteAsync(submittalQueueItem);
            
            
            var sendQueueItems = await CreateNotificationSendQueueItem(submittalQueueItem);

            var queueList = new List<SendQueueItem>();
            queueList.Add(sendQueueItems);

            return queueList;
        }

        protected async Task<string> GetInitialArchiveReferenceAsync(string distributionId)
        {
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(Guid.Parse(distributionId.ToUpper()));
            
            return distributionForm.InitialArchiveReference;
        }
        private async Task<SendQueueItem> CreateNotificationSendQueueItem(SubmittalQueueItem submittalQueueItem)
        {
            var sendQueueItems = new List<SendQueueItem>();
            string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";

            return new SendQueueItem()
            {
                ArchiveReference = ArchiveReference,
                ReceiverSequenceNumber = "0",
                Receiver = Receivers[0],
                Sender = Sender
            };
        }

        protected async Task CopyPDFToPublicBlobStorage(byte[] pdfDoc, string receiverName, string publicContainer, string receiversArchiveReference)
        {
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            var validReceiverFilename = new string(receiverName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
            
            var metadataList = new List<KeyValuePair<string, string>>();
            metadataList.Add(new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan"));
            metadataList.Add(new KeyValuePair<string, string>("ReceiversArchiveReference", receiversArchiveReference));
            await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Public,
                                                                publicContainer,
                                                                $"Uttalelse_{validReceiverFilename}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.pdf",
                                                                pdfDoc,
                                                                "application/pdf",
                                                                metadataList);
        }

        protected async Task<byte[]> GetPDFReplyFromPrivateBlobStorageAsync(string archiveReference)
        {
            var metadataList = new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan");

            return await _blobOperations.GetBlobAsBytesByMetadata(BlobStorageEnum.Private, archiveReference.ToLower(), metadataList);
        }

    }
}