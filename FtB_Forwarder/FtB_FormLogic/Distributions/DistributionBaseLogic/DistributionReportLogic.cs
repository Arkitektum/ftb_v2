using Altinn.Common;
using Altinn.Common.Exceptions;
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
        private readonly HtmlToPdfConverterHttpClient _htmlToPdfConverterHttpClient;
        private readonly INotificationAdapter _notificationAdapter;
        public DistributionReportLogic(IFormDataRepo repo, 
                                       ITableStorage tableStorage, 
                                       ILogger log,
                                       INotificationAdapter notificationAdapter, 
                                       IBlobOperations blobOperations,
                                       DbUnitOfWork dbUnitOfWork, 
                                       IHtmlUtils htmlUtils, 
                                       HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient)
            : base(repo, tableStorage, blobOperations, log, dbUnitOfWork)
        {
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

        public override async Task<string> ExecuteAsync(ReportQueueItem reportQueueItem)
        {
            var returnItem = await base.ExecuteAsync(reportQueueItem);

            await UpdateReceiverProcessStageAsync(reportQueueItem.ArchiveReference, reportQueueItem.ReceiverSequenceNumber, reportQueueItem.Receiver.Id, DistributionReceiverProcessStageEnum.ReadyForReporting);
            await AddToReceiverProcessLogAsync(reportQueueItem.ReceiverLogPartitionKey, reportQueueItem.Receiver.Id, ReceiverStatusLogEnum.ReadyForReporting);

            if (await ReadyForSubmittalReportingAsync(reportQueueItem))
            {
                await SendReceiptToSubmitterWhenAllReceiversAreProcessedAsync(reportQueueItem);
            }

            return returnItem;
        }

        private async Task SendReceiptToSubmitterWhenAllReceiversAreProcessedAsync(ReportQueueItem reportQueueItem)
        {
            try
            {
                _log.LogDebug($"Start SendReceiptToSubmitterWhenAllReceiversAreProcessed. Queue item: {reportQueueItem.ReceiverLogPartitionKey}");
                DistributionSubmittalEntity submittalEntity = await _tableStorage.GetTableEntityAsync<DistributionSubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                base._log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. All receivers has been processed.");
                var notificationMessage = new AltinnNotificationMessage();
                notificationMessage.ArchiveReference = ArchiveReference;
                notificationMessage.Receiver = GetReceiver();
                notificationMessage.ArchiveReference = reportQueueItem.ArchiveReference;
                _log.LogDebug("Start GetSubmitterReceiptMessage");
                var messageData = GetSubmitterReceiptMessage(reportQueueItem.ArchiveReference);
                notificationMessage.MessageData = messageData;
                _log.LogDebug("Start GetSubmitterReceipt");
                var plainReceiptHtml = await GetSubmitterReceipt(reportQueueItem);
                _log.LogDebug("Start Convert to PDF");
                byte[] PDFInbytes = await _htmlToPdfConverterHttpClient.Get(plainReceiptHtml);
                _log.LogDebug("Converted to PDF");
                var receiptAttachment = new AttachmentBinary()
                {
                    BinaryContent = PDFInbytes,
                    Filename = "Kvittering.pdf",
                    Name = "Kvittering",
                    ArchiveReference = ArchiveReference
                };
                _log.LogDebug("Get from blob storage");
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
                _log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt (notification).");
                _log.LogDebug("Start SendNotification");
                IEnumerable<DistributionResult> result = _notificationAdapter.SendNotification(notificationMessage);
                
                var sendingFailed = result.Any(x => x.DistributionComponent.Equals(DistributionComponent.Correspondence)
                                                   && (
                                                          x.Step.Equals(DistributionStep.Failed)
                                                        || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                        || x.Step.Equals(DistributionStep.UnkownErrorOccurred)
                                                        ));

                if (!sendingFailed)
                {
                    var updatedSubmittalEntity = _tableStorage.UpdateEntityRecordAsync<DistributionSubmittalEntity>(submittalEntity);
                    _log.LogDebug("Start Update all receiver entities");
                    var allReceivers = await _tableStorage.GetTableEntitiesAsync<DistributionReceiverEntity>(reportQueueItem.ArchiveReference.ToLower());
                    allReceivers.ToList().ForEach(x => x.ProcessStage = Enum.GetName(typeof(DistributionReceiverProcessStageEnum), DistributionReceiverProcessStageEnum.Completed));
                    await UpdateEntitiesAsync(allReceivers);
                    _log.LogDebug("Start BulkAddLogEntryToReceivers");
                    await BulkAddLogEntryToReceiversAsync(reportQueueItem.ArchiveReference, ReceiverStatusLogEnum.Completed);
                    _log.LogDebug("End SendReceiptToSubmitterWhenAllReceiversAreProcessed");
                    //await _blobOperations.ReleaseContainerLease(reportQueueItem.ArchiveReference.ToLower());
                }
                else
                {
                    var failedStep = result.Where(x => x.Step.Equals(DistributionStep.Failed)
                                                        || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                        || x.Step.Equals(DistributionStep.UnkownErrorOccurred)).Select(y => y.Step).First();
                    throw new SendNotificationException("Error: Failed during sending of submittal receipt", failedStep);
                }
            }
            catch (SendNotificationException ex)
            {
                base._log.LogError($"{GetType().Name}. Error: {ex.Text}: {ex.DistriutionStep}");
                throw ex;
            }
            catch (Exception ex)
            {
                base._log.LogError(ex, "Error occurred when creating and sending receipt");
                throw;
            }
            finally
            {
                await _blobOperations.ReleaseContainerLease(reportQueueItem.ArchiveReference.ToLower());
            }
        }

        protected virtual (string Filename, string Name) GetFileNameForMainForm()
        {
            throw new NotImplementedException();
        }
    }
}
