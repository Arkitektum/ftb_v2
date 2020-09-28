using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
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
            SubmittalEntity entity = new SubmittalEntity(archiveReference, receiverCount, DateTime.Now);
            _tableStorage.InsertEntityRecordAsync(entity, "ftbSubmittals");
        }

        public virtual List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
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
        protected void RemoveDuplicateReceivers()
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