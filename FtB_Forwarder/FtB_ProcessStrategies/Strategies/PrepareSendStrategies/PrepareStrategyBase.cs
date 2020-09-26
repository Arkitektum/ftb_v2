using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public abstract class PrepareStrategyBase : StrategyBase, IStrategy<List<SendQueueItem>, SubmittalQueueItem>
    {
        private readonly ITableStorage _tableStorage;
        public PrepareStrategyBase(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
        }

        private void CreateSubmittalDatabaseStatus(string archiveReference, int receiverCount)
        {
            SubmittalEntity entity = new SubmittalEntity();
            entity.PartitionKey = archiveReference;
            entity.RowKey = archiveReference;
            entity.ReceiverCount = receiverCount;
            _tableStorage.InsertSubmittalRecord(entity, "ftbSubmittals");
        }

        public virtual List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            RemoveDuplicateReceivers();
            CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers.Count);
            FormLogicBeingProcessed.ProcessPrepareStep();

            List<SendQueueItem> sendQueueItems = new List<SendQueueItem>();
            foreach (var receiverVar in Receivers)
            {
                var receiver = new Receiver() { Type = receiverVar.Type, Id = receiverVar.Id };
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, Receiver = receiver });
            }

            return sendQueueItems;
        }
        private void RemoveDuplicateReceivers()
        {
            foreach (var receiver in FormLogicBeingProcessed.Receivers)
            {
                if (!Receivers.Contains(receiver)) //Remove duplicate receivers
                {
                    Receivers.Add(receiver);
                }
            }
        }
    }
}