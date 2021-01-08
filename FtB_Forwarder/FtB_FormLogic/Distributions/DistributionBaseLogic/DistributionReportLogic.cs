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
using Ftb_DbModels;
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
        private readonly FileDownloadHttpClient _fileDownloadHttpClient;
        private readonly INotificationAdapter _notificationAdapter;
        public DistributionReportLogic(IFormDataRepo repo, 
                                       ITableStorage tableStorage, 
                                       ILogger log,
                                       INotificationAdapter notificationAdapter, 
                                       IBlobOperations blobOperations,
                                       DbUnitOfWork dbUnitOfWork, 
                                       IHtmlUtils htmlUtils, 
                                       HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient,
                                       FileDownloadHttpClient fileDownloadHttpClient)
            : base(repo, tableStorage, blobOperations, log, dbUnitOfWork)
        {
            _htmlToPdfConverterHttpClient = htmlToPdfConverterHttpClient;
            _fileDownloadHttpClient = fileDownloadHttpClient;
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
            _dbUnitOfWork.SetArchiveReference(reportQueueItem.ArchiveReference);

            var returnItem = await base.ExecuteAsync(reportQueueItem);

            await UpdateReceiverProcessStageAsync(reportQueueItem.ArchiveReference, reportQueueItem.ReceiverSequenceNumber, reportQueueItem.Receiver.Id, DistributionReceiverProcessStageEnum.ReadyForReporting);
            await AddToReceiverProcessLogAsync(reportQueueItem.ReceiverLogPartitionKey, reportQueueItem.Receiver.Id, DistributionReceiverStatusLogEnum.ReadyForReporting);

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
                bool receiptAlreadySentToSubmitter = submittalEntity.Status == Enum.GetName(typeof(DistributionSubmittalStatusEnum), DistributionSubmittalStatusEnum.Distributed)
                                                  || submittalEntity.Status == Enum.GetName(typeof(DistributionSubmittalStatusEnum), DistributionSubmittalStatusEnum.ReceiptSentToSubmitter);

                
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

                if (!receiptAlreadySentToSubmitter)
                {
                    _log.LogInformation($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt (notification).");
                    _log.LogDebug($"Start SendNotification to receiverId {notificationMessage.Receiver.Id}");
                    IEnumerable<DistributionResult> result = _notificationAdapter.SendNotification(notificationMessage);

                    var sendingSucceded = !result.Any(x => x.DistributionComponent.Equals(DistributionComponent.Correspondence)
                                                       && (
                                                              x.Step.Equals(DistributionStep.Failed)
                                                            || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                            || x.Step.Equals(DistributionStep.UnkownErrorOccurred)
                                                            ));
                    if (!sendingSucceded)
                    {
                        var failedStep = result.Where(x => x.Step.Equals(DistributionStep.Failed)
                                                            || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                            || x.Step.Equals(DistributionStep.UnkownErrorOccurred)).Select(y => y.Step).First();

                        throw new SendNotificationException("Error: Failed during sending of submittal receipt", failedStep);
                    }

                    submittalEntity.Status = Enum.GetName(typeof(DistributionSubmittalStatusEnum), DistributionSubmittalStatusEnum.ReceiptSentToSubmitter);
                    await _tableStorage.UpdateEntityRecordAsync<DistributionSubmittalEntity>(submittalEntity);
                }

                if (!await FileDownloadStatusExists(reportQueueItem.ArchiveReference.ToUpper()))
                {
                    _log.LogDebug($"Inserting into FileDownloadStatus for {reportQueueItem.ArchiveReference.ToUpper()}");
                    var guid = Guid.NewGuid();
                    var mainFormFileDownload = await PersistBlobAndCreateFileDownload(reportQueueItem.ArchiveReference,
                                                                                        guid,
                                                                                        "Nabovarsel",
                                                                                        "nabovarsel.pdf",
                                                                                        FileTypesForDownload.Nabovarsel,
                                                                                        "application/pdf",
                                                                                        mainFormAttachment.BinaryContent);

                    var receiptFileDownload = await PersistBlobAndCreateFileDownload(reportQueueItem.ArchiveReference,
                                                                                        guid,
                                                                                        "Nabovarsel",
                                                                                        "nabovarsel_kvittering.pdf",
                                                                                        FileTypesForDownload.KvitteringNabovarsel,
                                                                                        "application/pdf",
                                                                                        receiptAttachment.BinaryContent);

                    bool postSuccessful = await AddToFileDownloads(reportQueueItem.ArchiveReference, mainFormFileDownload);
                    if (!postSuccessful)
                    {
                        throw new FileDownloadStatusException($"Insert i FileDownloadStatus feilet for {reportQueueItem.ArchiveReference} og {mainFormFileDownload.Filename}");
                    }

                    postSuccessful = await AddToFileDownloads(reportQueueItem.ArchiveReference, receiptFileDownload);
                    
                    if (!postSuccessful)
                    {
                        throw new FileDownloadStatusException($"Insert i FileDownloadStatus feilet for {reportQueueItem.ArchiveReference} og {receiptFileDownload.Filename}");
                    }
                }

                _log.LogDebug("Start Update all receiver entities");
                var allReceivers = await _tableStorage.GetTableEntitiesAsync<DistributionReceiverEntity>(reportQueueItem.ArchiveReference.ToLower());
                var updatedEntities = allReceivers.Select(receiver => { receiver.ProcessStage = Enum.GetName(typeof(DistributionReceiverProcessStageEnum), DistributionReceiverProcessStageEnum.Reported); return receiver; }).ToList();
                var success = await UpdateEntitiesAsync(updatedEntities);
                if (!success)
                {
                    throw new Exception($"Update av DistributionReceiverEntity til ProcessStage=Reported feilet for {reportQueueItem.ArchiveReference}");
                }
                _log.LogDebug("Start BulkAddLogEntryToReceivers");
                await BulkAddLogEntryToReceiversAsync(reportQueueItem.ArchiveReference, DistributionReceiverStatusLogEnum.Completed);
                
                //await _blobOperations.ReleaseContainerLease(reportQueueItem.ArchiveReference.ToLower());
                submittalEntity.Status = Enum.GetName(typeof(DistributionSubmittalStatusEnum), DistributionSubmittalStatusEnum.Distributed);
                await _tableStorage.UpdateEntityRecordAsync<DistributionSubmittalEntity>(submittalEntity);
                await ReportFormProcessStatus("Ok");
                _log.LogDebug("End SendReceiptToSubmitterWhenAllReceiversAreProcessed");
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


        public async Task<FileDownloadStatus> PersistBlobAndCreateFileDownload(string archiveReference,
                                                                        Guid guid,
                                                                        string formName,
                                                                        string fileName,
                                                                        FileTypesForDownload fileType,
                                                                        string mimeType,
                                                                        byte[] pdfByteStream)
        {
            _log.LogDebug($"Adding file {fileName} to blob storage {guid.ToString()}");
            await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Private, 
                                                             guid.ToString().ToLower(), 
                                                             fileName, 
                                                             pdfByteStream, 
                                                             mimeType,
                                                             null);

            var blobLink = $"{_blobOperations.GetBlobUri(BlobStorageEnum.Private)}{guid}/{fileName}";
            var fileRecord = new FileDownloadStatus(archiveReference.ToUpper(), guid, fileType, fileName, blobLink, mimeType, formName);

            return fileRecord;
        }

        private async Task<bool> AddToFileDownloads(string archiveReference, FileDownloadStatus fileDownload)
        {
            _log.LogDebug($"Adding record to FileDownloadStatus for {archiveReference.ToUpper()} and blob {fileDownload.BlobLink}");
            return await _fileDownloadHttpClient.Post(archiveReference, fileDownload);
        }

        private async Task<bool> FileDownloadStatusExists(string archiveReference)
        {
            var listOfFileDownloadRecords = await _fileDownloadHttpClient.GetAll(archiveReference);
            
            return listOfFileDownloadRecords != null && listOfFileDownloadRecords.ToList().Count > 0;
        }

        private async Task ReportFormProcessStatus(string status)
        {
            var formMetadata = await _dbUnitOfWork.FormMetadata.Get();
            formMetadata.Status = status;
            _dbUnitOfWork.FormMetadata.Update(formMetadata);
            await _dbUnitOfWork.FormMetadata.Save();
        }

        protected virtual (string Filename, string Name) GetFileNameForMainForm()
        {
            throw new NotImplementedException();
        }
    }
}
