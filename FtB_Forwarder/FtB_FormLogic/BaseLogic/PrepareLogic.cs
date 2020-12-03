using FtB_Common;
using FtB_Common.BusinessLogic;
using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
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

        

        public PrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) 
            : base(repo, tableStorage, log, dbUnitOfWork)
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

            await CreateSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Receivers.Count);

            var sendQueueItems = new List<SendQueueItem>();

            //Bulk add receivers to database
            //FUN FACT: Since the partition key differs for all receivers a true bulk operation cannot be performed..
            var receiverEntities = new List<ReceiverEntity>();
            var receiverLogEntities = new List<ReceiverLogEntity>();
            for (int i = 0; i < Receivers.Count; i++)
            {
                string receiverLogPartitionKey = $"{ArchiveReference}-{i.ToString()}";
                string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";
                
                receiverEntities.Add(new ReceiverEntity(ArchiveReference, i.ToString(), Receivers[i].Id, ReceiverProcessStageEnum.Created, DateTime.Now, receiverLogPartitionKey));
                receiverLogEntities.Add(new ReceiverLogEntity(receiverLogPartitionKey, rowKey, Receivers[i].Id, ReceiverStatusLogEnum.Created));
                
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, ReceiverSequenceNumber = i.ToString(),
                                                         ReceiverLogPartitionKey = receiverLogPartitionKey, Receiver = Receivers[i] });
            }

            _log.LogDebug($"Created {receiverEntities.Count()} receiver entities for {submittalQueueItem.ArchiveReference}");

            await BulkInsertEntities(receiverEntities);
            await ParallelInsertEntities(receiverLogEntities);

            return sendQueueItems;
        }

        private async Task CreateSubmittalDatabaseStatus(string archiveReference, int receiverCount)
        {
            try
            {
                var entity = new SubmittalEntity(archiveReference, receiverCount, DateTime.Now);
                await _tableStorage.InsertEntityRecordAsync<SubmittalEntity>(entity);
                _log.LogDebug($"Create submittal database status for {archiveReference} with receiver count: {receiverCount}.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error creating submittal record for archiveReference={archiveReference}.");
                throw;
            }
        }
    }
}
