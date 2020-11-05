using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    public abstract class PrepareLogic<T> : LogicBase<T>, IFormLogic<IEnumerable<SendQueueItem>, SubmittalQueueItem>
    {
        protected virtual List<Receiver> Receivers { get; set; }

        public PrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork) : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        //public virtual IEnumerable<SendQueueItem> Execute(SubmittalQueueItem submittalQueueItem)
        //{
        //    throw new NotImplementedException();
        //}
        public virtual IEnumerable<SendQueueItem> Execute(SubmittalQueueItem submittalQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Processing logic for archveReference {submittalQueueItem.ArchiveReference}....");
            
            base.LoadData(submittalQueueItem.ArchiveReference);

            CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers.Count);

            var sendQueueItems = new List<SendQueueItem>();

            

            foreach (var receiverVar in Receivers)
            {
                var storageRowKey = Guid.NewGuid().ToString();
                CreateReceiverDatabaseStatus(submittalQueueItem.ArchiveReference, storageRowKey, receiverVar);
                //var receiver = new Receiver() { Type = receiverVar.Type, Id = receiverVar.Id };
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, StorageRowKey= storageRowKey, Receiver = receiverVar });
            }

            return sendQueueItems;
        }

        private void CreateSubmittalDatabaseStatus(string archiveReference, int receiverCount)
        {
            try
            {
                SubmittalEntity entity = new SubmittalEntity(archiveReference, receiverCount, DateTime.Now);
                _tableStorage.InsertEntityRecordAsync<SubmittalEntity>(entity);
                _log.LogDebug($"Create submittal database status for {archiveReference} with receiver count: {receiverCount}.");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating submittal record for archiveReference={archiveReference}. Message: {ex.Message}");
                throw ex;
            }
        }

        private void CreateReceiverDatabaseStatus(string archiveReference, string storageRowKey, Receiver receiver)
        {
            try
            {
                //ReceiverEntity entity = new ReceiverEntity(archiveReference, receiver.Id.Replace("/", ""), ReceiverStatusEnum.Created, DateTime.Now);
                ReceiverEntity entity = new ReceiverEntity(archiveReference, storageRowKey, receiver.Id, ReceiverStatusEnum.Created, DateTime.Now);
                _tableStorage.InsertEntityRecordAsync<ReceiverEntity>(entity);
                _log.LogInformation($"Create receiver database status for {archiveReference} and reciverId={receiver.Id}.");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating receiver records for archiveReference={archiveReference} and reciverId={receiver.Id}. Message: {ex.Message}");
                throw ex;
            }
        }

        protected abstract void GetReceivers();

        protected void RemoveDuplicateReceivers()
        {
            foreach (var receiver in this.Receivers)
            {
                if (!Receivers.Contains(receiver)) //Remove duplicate receivers
                {
                    Receivers.Add(receiver);
                }
            }
        }


    }
}
