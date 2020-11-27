using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Ftb_Repositories;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class LogicBase<T>
    {
        protected readonly ITableStorage _tableStorage;
        protected readonly ILogger _log;
        protected readonly DbUnitOfWork _dbUnitOfWork;

        public LogicBase(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork)
        {
            _repo = repo;
            _tableStorage = tableStorage;
            _log = log;
            _dbUnitOfWork = dbUnitOfWork;
        }
        public string ArchiveReference { get; set; }
        protected readonly IFormDataRepo _repo;
        public T FormData { get; set; }
        public async Task LoadData(string archiveReference)
        {
            _log.LogDebug($"{GetType().Name}: LoadFormData for ArchiveReference {archiveReference}....");
            ArchiveReference = archiveReference;
            var data = await _repo.GetFormData(ArchiveReference);
            FormData = SerializeUtil.DeserializeFromString<T>(data);
        }

        protected void ParallelInsertEntities(IEnumerable<ReceiverLogEntity> entities)
        {
            Parallel.ForEach(entities, entity =>
            {
                _tableStorage.InsertEntityRecord<ReceiverLogEntity>(entity);
            });
        }

        protected void BulkInsertEntities(IEnumerable<ReceiverEntity> entities)
        {
            _tableStorage.InsertEntityRecords<ReceiverEntity>(entities);            
        }

        public ReceiverStatusLogEnum GetReceiverLastLogStatus(string partitionKey)
        {
            var receiverRows = _tableStorage.GetTableEntities<ReceiverLogEntity>(partitionKey);
            var lastRow = receiverRows.OrderByDescending(x => x.RowKey).First();

            return (ReceiverStatusLogEnum)Enum.Parse(typeof(ReceiverStatusLogEnum), lastRow.Status);
        }

        public string GetReceiverIDFromStorage(string partitionKey, string rowKey)
        {
            var receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(partitionKey, rowKey);
            return receiverEntity.ReceiverId;
        }

        protected void BulkAddLogEntryToReceivers(ReportQueueItem reportQueueItem, ReceiverStatusLogEnum statusEnum)
        {
            _log.LogDebug("Inside BulkAddLogEntryToReceivers - start");
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference.ToLower(), reportQueueItem.ArchiveReference.ToLower());
            var totalNumberOfReceivers = submittalEntity.ReceiverCount;
            for (int i = 0; i < totalNumberOfReceivers; i++)
            {
                AddToReceiverProcessLog(reportQueueItem.ArchiveReference.ToLower(), $"{reportQueueItem.ArchiveReference.ToLower()}-{i.ToString()}", GetReceiverIDFromStorage(reportQueueItem.ArchiveReference.ToLower(), i.ToString()), statusEnum);
            }
            _log.LogDebug("Inside BulkAddLogEntryToReceivers - end");
        }

        protected void UpdateEntities<T>(IEnumerable<T> entities) where T : ITableEntity
        {
            _tableStorage.UpdateEntities<T>(entities);
        }


        protected void UpdateReceiverProcessStage(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverProcessStageEnum processStageEnum)
        {
            try
            {
                var receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(archiveReference, receiverSequenceNumber);
                receiverEntity.ProcessStage = Enum.GetName(typeof(ReceiverProcessStageEnum), processStageEnum);
                var result = _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                _log.LogDebug($"ID={archiveReference}. Updated receiver status for receiverSequenceNumber {receiverSequenceNumber} and receiverID {receiverID}. Status: {Enum.GetName(typeof(ReceiverProcessStageEnum), processStageEnum)}.....");
            }
            catch (Exception ex)
            {
                _log.LogError($"UpdateReceiverProcessStage: ArchveReference={archiveReference}. Error adding receiver record for ID={receiverSequenceNumber} and receiverID {receiverID}. Message: { ex.Message }");
                throw ex;
            }
        }
        protected void UpdateReceiverProcessOutcome(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverProcessOutcomeEnum processOutcomeEnum)
        {
            try
            {
                var receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(archiveReference, receiverSequenceNumber);
                receiverEntity.ProcessOutcome = Enum.GetName(typeof(ReceiverProcessOutcomeEnum), processOutcomeEnum);
                var result = _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                _log.LogDebug($"ID={archiveReference}. Updated receiver status for receiverSequenceNumber {receiverSequenceNumber} and receiverID {receiverID}. Status: {Enum.GetName(typeof(ReceiverProcessStageEnum), processOutcomeEnum)}.....");
            }
            catch (Exception ex)
            {
                _log.LogError($"UpdateReceiverProcessOutcome: ArchveReference={archiveReference}. Error adding receiver record for ID={receiverSequenceNumber} and receiverID {receiverID}. Message: { ex.Message }");
                throw ex;
            }
        }

        protected void AddToReceiverProcessLog(string archiveReference, string receiverPartitionKey, string receiverID, ReceiverStatusLogEnum statusEnum)
        {
            try
            {
                ReceiverLogEntity receiverEntity = new ReceiverLogEntity(receiverPartitionKey, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", receiverID, statusEnum);
                _log.LogDebug($"ID={receiverPartitionKey}. Added receiver status for {archiveReference} and receiverID {receiverID}. Status: {receiverEntity.Status}.....");
                _tableStorage.InsertEntityRecord<ReceiverLogEntity>(receiverEntity);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error adding receiver record for ID={receiverPartitionKey} and receiverID {receiverID}. ArchveReference={archiveReference}. Message: { ex.Message }");
                throw ex;
            }
        }
    }
}
