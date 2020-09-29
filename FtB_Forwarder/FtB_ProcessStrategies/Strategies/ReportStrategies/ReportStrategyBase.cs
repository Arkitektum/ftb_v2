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

namespace FtB_ProcessStrategies
{
    public abstract class ReportStrategyBase : StrategyBase, IStrategy<FinishedQueueItem, ReportQueueItem>
    {
        private readonly ITableStorage _tableStorage;
        private readonly IEnumerable<IMessageManager> _messageManagers;
        private readonly ILogger _log;

        public ReportStrategyBase(IFormLogic formLogic, ITableStorage tableStorage, IEnumerable<IMessageManager> messageManagers, ILogger log) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
            _messageManagers = messageManagers;
            _log = log;
        }
        private void IncrementSubmittalSentCount(string archiveReference, string receiverId)
        {
            bool runAgain = false;
            do
            {
                runAgain = false;
                try
                {
                    SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
                    _log.LogDebug($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ receiverId }. Before Increment for {submittalEntity.RowKey}. SentCount: { submittalEntity.SentCount }. ETag: { submittalEntity.ETag }");
                    submittalEntity.SentCount++;
                    //_tableStorage.InsertSubmittalRecordAsync(submittalEntity, "ftbSubmittals");

                    //Log the record to be inserted
                    _log.LogDebug($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ receiverId }. After Increment for {submittalEntity.RowKey}. SentCount: { submittalEntity.SentCount }. ETag: { submittalEntity.ETag }");
                    var updatedEntity = _tableStorage.UpdateEntityRecord(submittalEntity, "ftbSubmittals");
                    _log.LogDebug($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ receiverId }. After Update for {updatedEntity.RowKey}. ETag: { updatedEntity.ETag }");

                }
                catch (TableStorageConcurrentException ex)
                {
                    if (ex.HTTPStatusCode == 412)
                    {
                        int randomNumber = new Random().Next(0, 1000);
                        _log.LogInformation($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ receiverId }. Optimistic concurrency violation – entity has changed since it was retrieved. Run again after { randomNumber.ToString() } ms.");
                        Thread.Sleep(randomNumber);
                        runAgain = true;
                    }
                    else
                    {
                        _log.LogError($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: Error incrementing submittal record for ID={ receiverId }. Message: { ex.Message }");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: Error incrementing submittal record for ID={ receiverId }. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }

        private bool AllReceiversHasBeenSentTo(string archiveReference)
        {
            //TODO: This method has to return value based on status of sending to each separate reciver, and not on this "submittalEntity.SentCount"
            var submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
            return submittalEntity.SentCount == submittalEntity.ReceiverCount;
        }


        public virtual FinishedQueueItem Exceute(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            IncrementSubmittalSentCount(reportQueueItem.ArchiveReference, reportQueueItem.Receiver.Id);
            _log.LogInformation($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ reportQueueItem.Receiver.Id }. ReportStrategyBase function processed message");
            if (AllReceiversHasBeenSentTo(reportQueueItem.ArchiveReference))
            {
                _log.LogInformation($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ reportQueueItem.Receiver.Id }. ReportStrategyBase: All receivers has been processed. Sending to Slack.");
                foreach (var messageManager in _messageManagers)
                {
                    if (messageManager is SlackManager)
                    {
                        //Report on Slack channel: reportQueueItem.Receivers for reportQueueItem.ArchiveReference
                        StringBuilder sb = new StringBuilder();
                        sb.Append($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}:{ Environment.NewLine }Mottakere for arkivreferanse { reportQueueItem.ArchiveReference } er: { Environment.NewLine }");
                        var list = Receivers;
                        foreach (var receiver in list)
                        {
                            sb.Append($"Type\t{ receiver.Type }, Id\t{ receiver.Id }{ Environment.NewLine }");
                        }
                        _log.LogDebug(sb.ToString());
                        messageManager.Send(sb.ToString());
                    }
                }
            }
            else
            {
                _log.LogDebug($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ID={ reportQueueItem.Receiver.Id }. ReportStrategyBase: NOT all receivers has been processed.");
            }
            return null;
            
        }
    }
}