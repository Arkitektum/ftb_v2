using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
        private void IncrementSubmittalSentCount(string archiveReference)
        {
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
            submittalEntity.SentCount++;
            _tableStorage.InsertSubmittalRecord(submittalEntity, "ftbSubmittals");
        }

        private bool AllReceiversHasBeenSentTo(string archiveReference)
        {
            //TODO: This method has to return value based on status of sending to each separate reciver, and not on this "submittalEntity.SentCount"
            var submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
            return submittalEntity.SentCount == submittalEntity.ReceiverCount;
        }

        public virtual List<FinishedQueueItem> ExceuteAndReturnList(ReportQueueItem reportQueueItem)
        {
            throw new System.NotImplementedException();
        }

        public FinishedQueueItem Exceute(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            IncrementSubmittalSentCount(reportQueueItem.ArchiveReference);
            _log.LogInformation($"ReportStrategyBase function processed message: {reportQueueItem}");
            if (AllReceiversHasBeenSentTo(reportQueueItem.ArchiveReference))
            {
                _log.LogInformation($"ReportStrategyBase: All receivers has been processed. Sending to Slack");
                foreach (var messageManager in _messageManagers)
                {
                    if (messageManager is SlackManager)
                    {
                        //Report on Slack channel: reportQueueItem.Receivers for reportQueueItem.ArchiveReference
                        StringBuilder sb = new StringBuilder();
                        sb.Append($"Mottakere for arkivreferanse { reportQueueItem.ArchiveReference } er: { Environment.NewLine }");
                        var list = FormLogicBeingProcessed.Receivers;
                        foreach (var receiver in list)
                        {
                            sb.Append($"Type\t{ receiver.Type }, Id\t{ receiver.Id }");
                        }
                        messageManager.Send(sb.ToString());
                    }
                }
            }
            else
            {
                _log.LogInformation($"ReportStrategyBase: NOT all receivers has been processed.");
            }
            return null;
            
        }
    }
}