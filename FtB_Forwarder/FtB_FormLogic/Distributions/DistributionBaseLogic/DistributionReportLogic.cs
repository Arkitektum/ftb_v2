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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FtB_FormLogic
{
    public class DistributionReportLogic<T> : ReportLogic<T>
    {
        private readonly IBlobOperations _blobOperations;
        private readonly INotificationAdapter _notificationAdapter;
        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, INotificationAdapter notificationAdapter, IBlobOperations blobOperations, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
            _blobOperations = blobOperations;
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
        protected virtual string GetSubmitterReceipt(string archiveReference)
        {
            throw new NotImplementedException();
        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            var returnItem = base.Execute(reportQueueItem);
            ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);
            _log.LogDebug($"{GetType().Name}. Execute: ID={reportQueueItem.ArchiveReference}. RowKey={reportQueueItem.StorageRowKey}. ReceiverEntityStatus: {receiverEntity.Status}.");
            Enum.TryParse(receiverEntity.Status, out ReceiverStatusEnum receiverStatus);
            if (receiverEntity.Status != Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.ReadyForReporting))
            {
                UpdateSubmittalEntityAfterReceiverIsProcessed(reportQueueItem, receiverStatus);
            }
            SendReceiptToSubmitterWhenAllReceiversAreProcessed(reportQueueItem);

            return returnItem;
        }
        protected void UpdateSubmittalEntityAfterReceiverIsProcessed(ReportQueueItem reportQueueItem, ReceiverStatusEnum receiverStatus)
        {
            bool runAgain;
            string archiveReference = reportQueueItem.ArchiveReference;
            do
            {
                runAgain = false;
                try
                {
                    SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(archiveReference, archiveReference);
                    _log.LogDebug($"ID={archiveReference}. Before SubmittalEntity update for archiveRefrrence {archiveReference}. SubmittalEntityStatus: {submittalEntity.Status}. ReceiverStatusEnum: {receiverStatus}.");
                    submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Processing);
                    submittalEntity.ProcessedCount++;
                    switch (receiverStatus)
                    {
                        case ReceiverStatusEnum.DigitalDisallowment:
                            submittalEntity.DigitalDisallowmentCount++;
                            break;
                        case ReceiverStatusEnum.CorrespondenceSent:
                            submittalEntity.SuccessCount++;
                            break;
                        default:
                            submittalEntity.FailedCount++;
                            break;
                    }
                    _log.LogDebug($"ArchiveReference={archiveReference}. Updating  submittal. Status: Success: {submittalEntity.SuccessCount}, DigitalDisallowment: {submittalEntity.DigitalDisallowmentCount}, FailedCount: {submittalEntity.FailedCount}");
                    var updatedSubmittalEntity = _tableStorage.UpdateEntityRecord<SubmittalEntity>(submittalEntity);
                    ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);
                    receiverEntity.Status = Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.ReadyForReporting);
                    var updatedReceiverEntity = _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                }
                catch (TableStorageConcurrentException ex)
                {
                    if (ex.HTTPStatusCode == 412)
                    {
                        int randomNumber = new Random().Next(0, 1000);
                        _log.LogInformation($"ArchiveReference={archiveReference}. Optimistic concurrency violation – entity has changed since it was retrieved. Run again after { randomNumber.ToString() } ms.");
                        Thread.Sleep(randomNumber);
                        runAgain = true;
                    }
                    else
                    {
                        _log.LogError($"Error updating submittal record for ArchiveReference={archiveReference}. Message: {ex.Message}");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error updating submittal record for ArchiveReference={archiveReference}. Message: {ex.Message}");
                    throw ex;
                }
            } while (runAgain);

        }
        private void SendReceiptToSubmitterWhenAllReceiversAreProcessed(ReportQueueItem reportQueueItem)
        {
            try
            {
                SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                _log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. ID={reportQueueItem.Receiver.Id}. SubmittalEntity.ProcessedCount={submittalEntity.ProcessedCount}, submittalEntity.ReceiverCount={submittalEntity.ReceiverCount}");
                if (submittalEntity.ProcessedCount == submittalEntity.ReceiverCount)
                {
                    submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                    _log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. All receivers has been processed.");
                    var notificationMessage = new AltinnNotificationMessage();
                    notificationMessage.ArchiveReference = ArchiveReference;
                    notificationMessage.Receiver = GetReceiver();
                    notificationMessage.ArchiveReference = reportQueueItem.ArchiveReference;
                    var messageData = GetSubmitterReceiptMessage(reportQueueItem.ArchiveReference);
                    notificationMessage.MessageData = messageData;
                    var plainReceiptHtml = GetSubmitterReceipt(reportQueueItem.ArchiveReference);
                    byte[] PDFInbytes = HtmlUtils.GetPDFFromHTML(plainReceiptHtml);
                    
                    var receiptAttachment = new AttachmentBinary()
                    {
                        BinaryContent = PDFInbytes,
                        Filename = "Kvittering.pdf",
                        Name = "Kvittering",
                        ArchiveReference = ArchiveReference
                    };
                    var metadataList = new List<KeyValuePair<string, string>>();
                    metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));
                    var mainFormFromBlobStorage = _blobOperations.GetBlobsAsBytesByMetadata(ArchiveReference, metadataList);

                    var mainFormAttachment = new AttachmentBinary()
                    {
                        BinaryContent = mainFormFromBlobStorage.First(),
                        Filename = GetFileNameForMainForm().Filename,
                        Name = GetFileNameForMainForm().Name,
                        ArchiveReference = ArchiveReference
                    };

                    notificationMessage.Attachments = new List<Attachment>() { receiptAttachment, mainFormAttachment };
                    _log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt (notification).");
                    _notificationAdapter.SendNotification(notificationMessage);
                    _log.LogDebug($"{GetType().Name}: {plainReceiptHtml}");
                    var updatedSubmittalEntity = _tableStorage.UpdateEntityRecord<SubmittalEntity>(submittalEntity);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");
                throw ex;
            }
        }

        protected virtual (string Filename, string Name) GetFileNameForMainForm()
        {
            throw new NotImplementedException();
        }
    }
}
