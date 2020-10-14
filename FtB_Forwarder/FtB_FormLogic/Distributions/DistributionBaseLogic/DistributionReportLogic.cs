using Altinn.Common.Models;
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
    public class DistributionReportLogic<T> : ReportLogic<T>
    {

        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IEnumerable<IMessageManager> messageManagers) 
            : base(repo, tableStorage, log)
        {
            _messageManagers = messageManagers;

        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Execute.....");
            var returnItem =  base.Execute(reportQueueItem);
            //var receiverEntity = new ReceiverEntity(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);
            ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>("ftbReceivers", reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);

            _log.LogDebug($"{GetType().Name}. Execute: ID={reportQueueItem.ArchiveReference}. RowKey={reportQueueItem.StorageRowKey}. ReceiverEntityStatus: {receiverEntity.Status}.");
            Enum.TryParse(receiverEntity.Status, out ReceiverStatusEnum receiverStatus);

            if (receiverEntity.Status != Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.ReadyForReporting))
            {
                UpdateSubmittalEntityAfterReceiverIsProcessed(reportQueueItem, receiverStatus);
            }
            SendReceiptToSubmitterWhenAllReceiversAreProcessed(reportQueueItem);




            //Sende kvittering til innsender



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

                    _log.LogDebug($"ArchiveReference={archiveReference}. Updating  submittal. Status: Success: {submittalEntity.SuccessCount}, DigitalDisallowment: {submittalEntity.DigitalDisallowmentCount}, FailedCount: {submittalEntity.FailedCount}");
                    var updatedEntity = _tableStorage.UpdateEntityRecord(submittalEntity, "ftbSubmittals");

                    UpdateReceiverEntity(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey, ReceiverStatusEnum.ReadyForReporting);

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
        private void SendReceiptToSubmitterWhenAllReceiversAreProcessed(ReportQueueItem reportQueueItem)
        {
            try
            {

                SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                _log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. ID={reportQueueItem.Receiver.Id}. SubmittalEntity.ProcessedCount={submittalEntity.ProcessedCount}, submittalEntity.ReceiverCount={submittalEntity.ReceiverCount}");
                if (submittalEntity.ProcessedCount == submittalEntity.ReceiverCount)
                {
                    submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                    //TODO: Must create a submitter report
                    _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. ReportStrategyBase: All receivers has been processed.");
                    foreach (var messageManager in _messageManagers)
                    {
                        if (messageManager is SlackManager)
                        {
                            //Report on Slack channel
                            var messageData = GetSubmitterReceipt(reportQueueItem.ArchiveReference);
                            string message = messageData.MessageTitle + Environment.NewLine
                                            + messageData.MessageSummary + Environment.NewLine
                                            + messageData.MessageBody;
                            //Formatting for Slack
                            message = message.Replace("<br>", "\n").Replace("<p>", "\n").Replace("</p>", "");
                            _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}. Sending to Slack.");
                            _log.LogDebug($"{GetType().Name}: {message}");
                            messageManager.Send(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");
                throw ex;
            }
        }

        protected virtual MessageDataType GetSubmitterReceipt(string archiveReference) 
        {
            throw new NotImplementedException();
        }

        private bool AllReceiversHasBeenProcessed(string archiveReference)
        {
            //TODO: This method has to return value based on status of sending to each separate reciver, and not on this "submittalEntity.SentCount"
            var submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
            return submittalEntity.ProcessedCount == submittalEntity.ReceiverCount;
        }
    }
}
