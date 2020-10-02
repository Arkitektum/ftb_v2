using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public abstract class PrepareStrategyBase : StrategyBase, IStrategy<List<SendQueueItem>, SubmittalQueueItem>
    {
        private readonly ITableStorage _tableStorage;
        private readonly ILogger _log;

        public PrepareStrategyBase(ITableStorage tableStorage, ILogger log) : base(tableStorage, log)
        {
            _tableStorage = tableStorage;
            _log = log;
        }

        public virtual List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            //FormLogicBeingProcessed.InitiateForm();

            CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers.Count);
            CreateReceiversDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers);
            FormLogicBeingProcessed.ProcessPrepareStep();

            List<SendQueueItem> sendQueueItems = new List<SendQueueItem>();
            foreach (var receiverVar in Receivers)
            {
                var receiver = new Receiver() { Type = receiverVar.Type, Id = receiverVar.Id };
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, Receiver = receiver });
            }

            return sendQueueItems;
        }

        private void CreateSubmittalDatabaseStatus(string archiveReference, int receiverCount)
        {
            try
            {
                SubmittalEntity entity = new SubmittalEntity(archiveReference, receiverCount, DateTime.Now);
                _tableStorage.InsertEntityRecordAsync(entity, "ftbSubmittals");
                _log.LogDebug($"Create submittal database status for {archiveReference} with receiver count: {receiverCount}.");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating submittal record for archiveReference={archiveReference}. Message: {ex.Message}");
                throw ex;
            }
        }

        private void CreateReceiversDatabaseStatus(string archiveReference, List<Receiver> receivers)
        {
            foreach (var receiver in receivers)
            {
                try
                {
                    ReceiverEntity entity = new ReceiverEntity(archiveReference, receiver.Id.Replace("/",""), ReceiverStatusEnum.Created, DateTime.Now);
                    _tableStorage.InsertEntityRecordAsync(entity, "ftbReceivers");
                    _log.LogInformation($"Create receiver database status for {archiveReference} and reciverId={receiver.Id}.");
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error creating receiver records for archiveReference={archiveReference} and reciverId={receiver.Id}. Message: {ex.Message}");
                    throw ex;
                }
            }
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