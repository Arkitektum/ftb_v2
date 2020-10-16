using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class DistributionReportLogic<T> : ReportLogic<T>
    {
        private readonly INotificationAdapter _notificationAdapter;

        //private readonly IEnumerable<IMessageManager> _messageManagers;

        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, INotificationAdapter notificationAdapter) //, IEnumerable<IMessageManager> messageManagers) 
            : base(repo, tableStorage, log)
        {
            _notificationAdapter = notificationAdapter;
            //_messageManagers = messageManagers;

        }

        public virtual AltinnReceiver GetReceiver()
        {
            throw new System.NotImplementedException();
        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Execute.....");
            var returnItem = base.Execute(reportQueueItem);
            //var receiverEntity = new ReceiverEntity(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);
            ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey);

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

                    //UpdateReceiverEntity(reportQueueItem.ArchiveReference, reportQueueItem.StorageRowKey, ReceiverStatusEnum.ReadyForReporting);

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

                SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
                _log.LogDebug($"{GetType().Name}. ArchiveReference={reportQueueItem.ArchiveReference}. ID={reportQueueItem.Receiver.Id}. SubmittalEntity.ProcessedCount={submittalEntity.ProcessedCount}, submittalEntity.ReceiverCount={submittalEntity.ReceiverCount}");
                if (submittalEntity.ProcessedCount == submittalEntity.ReceiverCount)
                {
                    submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed);
                    _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. ReportStrategyBase: All receivers has been processed.");
                    //foreach (var messageManager in _messageManagers)
                    //{
                    //if (messageManager is SlackManager)
                    //{
                    //Report on Slack channel

                    var notificationMessage = new AltinnNotificationMessage();
                    notificationMessage.ArchiveReference = ArchiveReference;

                    notificationMessage.Receiver = GetReceiver();
                    notificationMessage.ArchiveReference = reportQueueItem.ArchiveReference;
                    var messageData = GetSubmitterReceiptMessage(reportQueueItem.ArchiveReference);
                    //string receiptMessage = messageData.MessageTitle + Environment.NewLine
                    //                + messageData.MessageSummary + Environment.NewLine
                    //                + messageData.MessageBody;

                    notificationMessage.MessageData = messageData;

                    //Formatting for Slack
                    //receiptMessage = receiptMessage.Replace("<br />", "\n").Replace("<p>", "\n").Replace("</p>", "");
                    _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt message to Slack.");
                    //_log.LogDebug($"{GetType().Name}: {receiptMessage}");


                    //string hopelessMessageSeparator = "\n\n\n\n\n\n\n\n\n\n";
                    //var slackMessage = Task.Run(async () => await messageManager.Send(receiptMessage));

                    //Create PDF from HTML 
                    var receipt = GetSubmitterReceipt(reportQueueItem.ArchiveReference);

                    var receiptAttachment = new AttachmentBinary()
                    {
                        BinaryContent = System.Text.Encoding.UTF8.GetBytes(receipt),
                        Filename = "Kvittering.html",
                        Name = "Kvitto",
                        SendersReference = ArchiveReference
                    };

                    notificationMessage.Attachments = new List<Attachment>() { receiptAttachment };
                    

                    _notificationAdapter.SendNotification(notificationMessage);

                    //receipt = receipt.Replace("<br />", "\n").Replace("<p>", "\n").Replace("</p>", "");
                    _log.LogInformation($"ArchiveReference={reportQueueItem.ArchiveReference}. Sending receipt to Slack.");
                    _log.LogDebug($"{GetType().Name}: {receipt}");
                    //Task.Run(async () => await messageManager.Send(receiptMessage + hopelessMessageSeparator + receipt));

                    //}
                    //}
                    //TODO: Update submittalstatus with Completed
                    var updatedSubmittalEntity = _tableStorage.UpdateEntityRecord<SubmittalEntity>(submittalEntity);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");
                throw ex;
            }
        }

        protected virtual MessageDataType GetSubmitterReceiptMessage(string archiveReference)
        {
            throw new NotImplementedException();
        }
        protected virtual string GetSubmitterReceipt(string archiveReference)
        {
            throw new NotImplementedException();
        }

        protected string AddTableOfAttachmentsToHtml(IEnumerable<Tuple<string, string>> attachments, string tableHeaderText)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("<div class='SubHeadingUnderline PaddingTop Paddingbottom'>" + tableHeaderText + "</div>");
            strBuilder.Append("<div class='Paragraf'>");
            strBuilder.Append("<table id='tabell'>");
            strBuilder.Append("<thead>");
            strBuilder.Append("<tr>");
            strBuilder.Append("<td><b>Vedleggstype:</b></td>");
            strBuilder.Append("<td><b>Filnavn:</b></td>");
            strBuilder.Append("</tr>");
            strBuilder.Append("</thead>");
            strBuilder.Append("<tbody>");

            foreach (var vedlegg in attachments)
            {
                strBuilder.Append("<tr>");
                strBuilder.Append("<td>" + vedlegg.Item1 + "</td>");
                strBuilder.Append("<td>" + vedlegg.Item2 + "</td>");
                strBuilder.Append("</tr>");
            }

            strBuilder.Append("</tbody>");
            strBuilder.Append("</table>");
            strBuilder.Append("</div>");

            return strBuilder.ToString();
        }

        //protected IEnumerable<Tuple<string,string> GetAttachmentListSentToReceivers(IEnumerable<BlobStorageMetadataTypeEnum> blobStorageTypes)
        //{
        //    //Get list of blobStorageTypes


        //    return new List<new Tuple<string, string>("","");
        //}
        private bool AllReceiversHasBeenProcessed(string archiveReference)
        {
            //TODO: This method has to return value based on status of sending to each separate reciver, and not on this "submittalEntity.SentCount"
            var submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(archiveReference, archiveReference);
            return submittalEntity.ProcessedCount == submittalEntity.ReceiverCount;
        }
    }
}
