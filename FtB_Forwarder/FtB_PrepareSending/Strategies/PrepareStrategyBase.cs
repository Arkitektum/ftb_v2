using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;

namespace FtB_PrepareSending.Strategies
{
    public abstract class PrepareStrategyBase : StrategyBase, IStrategy<SendQueueItem, SubmittalQueueItem>
    {
        private readonly ITableStorage _tableStorage;
        public PrepareStrategyBase(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
        }

        private void CreateSubmittalDatabaseStatus(string archiveReference, int receiverCount)
        {
            _tableStorage.InsertSubmittalRecord(new SubmittalEntity(archiveReference, receiverCount) , "ftbSubmittals");
        }

        public virtual List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            SetReceivers();
            
            CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, FormLogicBeingProcessed.Receivers.Count);

            FormLogicBeingProcessed.ProcessPrepareStep();
            return null;
        }
        private void SetReceivers()
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