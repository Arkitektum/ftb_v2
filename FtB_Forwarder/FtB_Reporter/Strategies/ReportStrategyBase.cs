using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using System.Collections.Generic;

namespace FtB_Reporter.Strategies
{
    public abstract class ReportStrategyBase : StrategyBase, IStrategy<FinishedQueueItem, ReportQueueItem>
    {
        private readonly ITableStorage _tableStorage;
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public ReportStrategyBase(IFormLogic formLogic, ITableStorage tableStorage, IEnumerable<IMessageManager> messageManagers) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
            _messageManagers = messageManagers;
        }
        private void IncrementSubmittalSentCount(string archiveReference)
        {
            //var submittalEntity = Task.Run(() => _tableStorage.GetTableEntity<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference)).Wait();
            //var task = _tableStorage.GetTableEntityAsync<SubmittalEntity>("ftbSubmittals", archiveReference, archiveReference);
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
            if (AllReceiversHasBeenSentTo(reportQueueItem.ArchiveReference))
            {
                //Report on Slack channel: reportQueueItem.Receivers for reportQueueItem.ArchiveReference
                foreach (var messageManager in _messageManagers)
                {
                    if (messageManager is SlackManager)
                    {
                        var list = FormLogicBeingProcessed.Receivers;
                        //list.
                        messageManager.Send("dasdasd");
                    }
                }
            }
            return null;
            
        }
    }
}