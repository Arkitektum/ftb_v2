using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class DistributionReportLogic<T> : ReportLogic<T>
    {
        private readonly IBlobOperations _blobOperations;
        private readonly HtmlToPdfConverterHttpClient _htmlToPdfConverterHttpClient;
        private readonly INotificationAdapter _notificationAdapter;
        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log
                                , INotificationAdapter notificationAdapter, IBlobOperations blobOperations
                                , DbUnitOfWork dbUnitOfWork, IHtmlUtils htmlUtils, HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient)
            : base(repo, tableStorage, tableStorageOperations, log, dbUnitOfWork)
        {
            _blobOperations = blobOperations;
            _htmlToPdfConverterHttpClient = htmlToPdfConverterHttpClient;
            _notificationAdapter = notificationAdapter;
        }

        public virtual AltinnReceiver GetReceiver()
        {
            throw new System.NotImplementedException();
        }
        protected virtual MessageDataType GetSubmitterReceiptMessage(string archiveReference)
        {
            throw new NotImplementedException();
        }
        protected virtual async Task<string> GetSubmitterReceipt(ReportQueueItem reportQueueItem)
        {
            throw new NotImplementedException();
        }

        public override async Task<string> Execute(ReportQueueItem reportQueueItem)
        {
            var returnItem = await base.Execute(reportQueueItem);
            AddReceiverProcessStatus(reportQueueItem.ArchiveReference, reportQueueItem.ReceiverSequenceNumber, reportQueueItem.Receiver.Id, ReceiverStatusEnum.ReadyForReporting);

            if (AreAllReceiversReadyForReporting(reportQueueItem))
            {
                await SendReceiptToSubmitterWhenAllReceiversAreProcessed(reportQueueItem);
            }

            return returnItem;
        }

        private async Task SendReceiptToSubmitterWhenAllReceiversAreProcessed(ReportQueueItem reportQueueItem)
        {
            try
            {
                SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                base._log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. ID={reportQueueItem.Receiver.Id}. SubmittalEntity.ProcessedCount={submittalEntity.ProcessedCount}, submittalEntity.ReceiverCount={submittalEntity.ReceiverCount}");
                submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                base._log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. All receivers has been processed.");
                var notificationMessage = new AltinnNotificationMessage();
                notificationMessage.ArchiveReference = ArchiveReference;
                notificationMessage.Receiver = GetReceiver();
                notificationMessage.ArchiveReference = reportQueueItem.ArchiveReference;
                var messageData = GetSubmitterReceiptMessage(reportQueueItem.ArchiveReference);
                notificationMessage.MessageData = messageData;
                var plainReceiptHtml = await GetSubmitterReceipt(reportQueueItem);
                byte[] PDFInbytes = _htmlToPdfConverterHttpClient.Get(plainReceiptHtml);
                    
                var receiptAttachment = new AttachmentBinary()
                {
                    BinaryContent = PDFInbytes,
                    Filename = "Kvittering.pdf",
                    Name = "Kvittering",
                    ArchiveReference = ArchiveReference
                };
                string publicContainerName = _blobOperations.GetPublicBlobContainerName(reportQueueItem.ArchiveReference.ToLower());
                var metadataList = new List<KeyValuePair<string, string>>();
                metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));
                var mainFormFromBlobStorage = _blobOperations.GetBlobsAsBytesByMetadata(BlobStorageEnum.Public, publicContainerName, metadataList);

                var mainFormAttachment = new AttachmentBinary()
                {
                    BinaryContent = mainFormFromBlobStorage.First(),
                    Filename = GetFileNameForMainForm().Filename,
                    Name = GetFileNameForMainForm().Name,
                    ArchiveReference = ArchiveReference
                };

                notificationMessage.Attachments = new List<Attachment>() { receiptAttachment, mainFormAttachment };
                base._log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt (notification).");
                _notificationAdapter.SendNotification(notificationMessage);
                base._log.LogDebug($"{GetType().Name}: {plainReceiptHtml}");
                var updatedSubmittalEntity = _tableStorage.UpdateEntityRecord<SubmittalEntity>(submittalEntity);
            }
            catch (Exception ex)
            {
                base._log.LogError($"{GetType().Name}. Error: {ex.Message}");
                throw ex;
            }
        }

        protected virtual (string Filename, string Name) GetFileNameForMainForm()
        {
            throw new NotImplementedException();
        }
    }
}
