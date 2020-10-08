using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<string, ReportQueueItem>, IReportLogic
    {
        protected virtual SubmitterReport SubmitterReport { get; set; }
        private readonly IEnumerable<IMessageManager> _messageManagers;


        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IEnumerable<IMessageManager> messageManagers) : base(repo, tableStorage, log)
        {
            SubmitterReport = new SubmitterReport();
            _messageManagers = messageManagers;
        }

        //protected abstract void SetSubmitterReportContent(SubmittalEntity submittalEntity);
        public virtual void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
            throw new NotImplementedException();
        }

        public virtual string Execute(ReportQueueItem reportQueueItem)
        {
            //var receiverEntity = new ReceiverEntity(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);
            ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>("ftbReceivers", reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);

            _log.LogDebug($"{GetType().Name}. Execute: ID={reportQueueItem.ArchiveReference}. RowKey={reportQueueItem.StorageRowKey}. ReceiverEntityStatus: {receiverEntity.Status}.");
            Enum.TryParse(receiverEntity.Status, out ReceiverStatusEnum receiverStatus);
            UpdateSubmittalEntityAfterReceiverIsProcessed(reportQueueItem.ArchiveReference, receiverStatus);
            SendReportWhenAllReceiversAreProcessed(reportQueueItem);
            return reportQueueItem.ArchiveReference;
        }

        protected void UpdateSubmittalEntityAfterReceiverIsProcessed(string archiveReference, ReceiverStatusEnum receiverStatus)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
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

                    //Log the record to be inserted
                    _log.LogDebug($"ArchiveReference={archiveReference}. Updating  submittal. Status: Success: {submittalEntity.SuccessCount}, DigitalDisallowment: {submittalEntity.DigitalDisallowmentCount}, FailedCount: {submittalEntity.FailedCount}");
                    var updatedEntity = _tableStorage.UpdateEntityRecord(submittalEntity, "ftbSubmittals");
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
                        _log.LogError($"Error updating submittal record for ArchiveReference={archiveReference}. Message: { ex.Message }");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error updating submittal record for ArchiveReference={archiveReference}. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }
        private void SendReportWhenAllReceiversAreProcessed(ReportQueueItem reportQueueItem)
        {
            try
            {
                
                SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                _log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. ID={reportQueueItem.Receiver.Id}.Before SubmittalEntity update for archiveReference. SubmittalStatus: {submittalEntity.Status}.");
                _log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. submittalEntity.ProcessedCount={submittalEntity.ProcessedCount}, submittalEntity.ReceiverCount={submittalEntity.ReceiverCount}");
                if (submittalEntity.ProcessedCount == submittalEntity.ReceiverCount)
                {
                    submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                    SetSubmitterReportContent(submittalEntity);
                    //TODO: Must create a submitter report
                    _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. ReportStrategyBase: All receivers has been processed.");
                    foreach (var messageManager in _messageManagers)
                    {
                        if (messageManager is SlackManager)
                        {
                            //Report on Slack channel
                            var message = SubmitterReport.Subject + Environment.NewLine + SubmitterReport.Body;
                            _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}. Sending to Slack.");
                            _log.LogDebug($"{GetType().Name}: {message}");
                            messageManager.Send(message);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private void IncrementSubmittalProcessedCount(string archiveReference, string receiverId)
        //{
        //    bool runAgain;
        //    do
        //    {
        //        runAgain = false;
        //        try
        //        {
        //            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
        //            _log.LogDebug($"ID={receiverId}. Before Increment for {submittalEntity.RowKey}. SentCount: {submittalEntity.ProcessedCount}. ETag: {submittalEntity.ETag}");
        //            submittalEntity.ProcessedCount++;
        //            //TODO: Should it be Async? _tableStorage.InsertSubmittalRecordAsync(submittalEntity, "ftbSubmittals");

        //            //Log the record to be inserted
        //            _log.LogDebug($"ID={receiverId}. After Increment for {submittalEntity.RowKey}. SentCount: {submittalEntity.ProcessedCount}. ETag: {submittalEntity.ETag}");
        //            var updatedEntity = _tableStorage.UpdateEntityRecord(submittalEntity, "ftbSubmittals");
        //            _log.LogDebug($"ID={receiverId}. After Update for {updatedEntity.RowKey}. ETag: {updatedEntity.ETag}");

        //        }
        //        catch (TableStorageConcurrentException ex)
        //        {
        //            if (ex.HTTPStatusCode == 412)
        //            {
        //                int randomNumber = new Random().Next(0, 1000);
        //                _log.LogInformation($"ID={receiverId}. Optimistic concurrency violation – entity has changed since it was retrieved. Run again after {randomNumber.ToString()} ms.");
        //                Thread.Sleep(randomNumber);
        //                runAgain = true;
        //            }
        //            else
        //            {
        //                _log.LogError($"Error incrementing submittal record for ID={receiverId}. Message: {ex.Message}");
        //                throw ex;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _log.LogError($"Error incrementing submittal record for ID={receiverId}. Message: {ex.Message}");
        //            throw ex;
        //        }
        //    } while (runAgain);
        //}

        private bool AllReceiversHasBeenProcessed(string archiveReference)
        {
            //TODO: This method has to return value based on status of sending to each separate reciver, and not on this "submittalEntity.SentCount"
            var submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
            return submittalEntity.ProcessedCount == submittalEntity.ReceiverCount;
        }


    }
}

