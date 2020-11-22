using FtB_Common;
using FtB_Common.BusinessLogic;
using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class PrepareLogic<T> : LogicBase<T>, IFormLogic<IEnumerable<SendQueueItem>, SubmittalQueueItem>
    {
        private readonly IDecryptionFactory _decryptionFactory;

        protected List<Receiver> _receivers;
        protected virtual List<Receiver> Receivers
        {
            //get { return _receivers.Distinct(new ReceiverEqualtiyComparer(_decryptionFactory)).ToList(); }
            get { return _receivers; }
            set { _receivers = value; }
        }

        

        public PrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) 
            : base(repo, tableStorage, tableStorageOperations, log, dbUnitOfWork)
        {
            _decryptionFactory = decryptionFactory;
        }

        public virtual void SetReceivers()
        {  }

        public void SetReceivers(IEnumerable<Receiver> receivers)
        {
            var comparrisonSource = new List<ReceiverInternal>();
            foreach (var receiver in receivers)
            {
                var receiverInternal = new ReceiverInternal(receiver);
                receiverInternal.DecryptedId = receiverInternal.Id.Length > 11 ? _decryptionFactory.GetDecryptor().DecryptText(receiverInternal.Id) : receiver.Id;
                comparrisonSource.Add(receiverInternal);
            }
            //_receivers = comparrisonSource.Distinct(new ReceiverEqualtiyComparer(_decryptionFactory)).ToList<Receiver>();
            var distinctList = comparrisonSource.Distinct(new ReceiverEqualtiyComparer()).ToList();
            _receivers = distinctList.Select(s => new Receiver() { Id = s.Id, Type = s.Type }).ToList();
        }

        public virtual async Task<IEnumerable<SendQueueItem>> Execute(SubmittalQueueItem submittalQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Processing logic for archveReference {submittalQueueItem.ArchiveReference}....");
            
            await base.LoadData(submittalQueueItem.ArchiveReference);

            SetReceivers();

            CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers.Count);

            var sendQueueItems = new List<SendQueueItem>();

            //Bulk add receivers to database
            //FUN FACT: Since the partition key differs for all receivers a true bulk operation cannot be performed..
            var receiverEntities = new List<ReceiverEntity>();
            for (int i = 0; i < Receivers.Count; i++)
            {
                receiverEntities.Add(new ReceiverEntity($"{ArchiveReference}-{i.ToString()}", $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", Receivers[i].Id, ReceiverStatusEnum.Created, DateTime.Now));
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, ReceiverSequenceNumber = i.ToString(), Receiver = Receivers[i] });
            }

            AddReceiversProcessStatus(receiverEntities);

            //int receiverSequenceNumber = 0;
            //foreach (var receiverVar in Receivers)
            //{
            //    CreateReceiverDatabaseStatus(submittalQueueItem.ArchiveReference, receiverSequenceNumber.ToString(), receiverVar);
            //    sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, ReceiverSequenceNumber = receiverSequenceNumber.ToString(), Receiver = receiverVar });
            //    receiverSequenceNumber++;
            //}

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

        private void CreateReceiverDatabaseStatus(string archiveReference, string receiverSequenceNumber, Receiver receiver)
        {
            try
            {                
                AddReceiverProcessStatus(archiveReference, receiverSequenceNumber, receiver.Id, ReceiverStatusEnum.Created);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating receiver records for archiveReference={archiveReference} and reciverId={receiver.Id}. Message: {ex.Message}");
                throw ex;
            }
        }
    }
}
