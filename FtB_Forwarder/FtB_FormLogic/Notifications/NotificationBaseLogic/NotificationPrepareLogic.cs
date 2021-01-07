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
using Ftb_DbModels;

namespace FtB_FormLogic
{
    public abstract class NotificationPrepareLogic<T> : PrepareLogic<T>
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
            var sendQueueItems = CreateNotificationSendQueueItem(submittalQueueItem);
            var queueList = new List<SendQueueItem>();
            queueList.Add(sendQueueItems);
            await UpdateDistributionForms();
            return queueList;
        }

        protected async Task<string> GetInitialArchiveReferenceAsync(string distributionId)
        {
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(distributionId.ToUpper());
            
            return distributionForm.InitialArchiveReference;
        }
        private SendQueueItem CreateNotificationSendQueueItem(SubmittalQueueItem submittalQueueItem)
        {
            return new SendQueueItem()
            {
                ArchiveReference = ArchiveReference,
                ReceiverSequenceNumber = "0",
                Receiver = Receivers[0],
                Sender = Sender
            };
        }

        public async Task UpdateDistributionForms()
        {
            var distributionId = GetHovedinnsendingsNummer();
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(distributionId.ToString());
            distributionForm.Signed = DateTime.Now;
            distributionForm.DistributionStatus = DistributionStatus.signed;
            distributionForm.SignedArchiveReference = ArchiveReference.ToUpper();

            await _dbUnitOfWork.DistributionForms.Update(distributionForm.InitialArchiveReference, distributionId, distributionForm);
        }

        protected async Task CopyPDFToPublicBlobStorage(byte[] pdfDoc, string senderName, string publicContainer, string sendersArchiveReference)
        {
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            var validSenderFilename = new string(senderName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
            
            var metadataList = new List<KeyValuePair<string, string>>();
            metadataList.Add(new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan"));
            metadataList.Add(new KeyValuePair<string, string>("SendersArchiveReference", sendersArchiveReference));
            await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Public,
                                                                publicContainer,
                                                                $"Uttalelse_{validSenderFilename}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.pdf",
                                                                pdfDoc,
                                                                "application/pdf",
                                                                metadataList);
        }

        protected async Task<byte[]> GetPDFReplyFromPrivateBlobStorageAsync(string archiveReference)
        {
            var metadataList = new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan");

            return await _blobOperations.GetBlobAsBytesByMetadata(BlobStorageEnum.Private, archiveReference.ToLower(), metadataList);
        }

        protected abstract Guid GetHovedinnsendingsNummer();

    }
}