using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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

        protected async Task ParallelInsertEntities(IEnumerable<ReceiverLogEntity> entities)
        {
            _log.LogDebug($"Parallel insert receiver log entities");
            await _tableStorage.EnsureTableExists<ReceiverLogEntity>();

            var list = entities.ToList();

            //Partitioner creates batches of elements. This makes a more predictable
            //way of performing parallel executions of logic
            var partitioner = Partitioner.Create(0, entities.Count(), 100);
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 8 };
            Parallel.ForEach(partitioner, options, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    _tableStorage.InsertEntityRecord<ReceiverLogEntity>(list[i]);
                }
            });


            //_log.LogDebug($"Ensures table exist for ReceiverLogEntity");
            //await _tableStorage.EnsureTableExists<ReceiverLogEntity>();

            //_log.LogDebug($"Creates tasks for inserts");
            //var tasks = entities.Select(s => _tableStorage.InsertEntityRecord<ReceiverLogEntity>(s));
            //var batchSize = 100;

            //_log.LogDebug($"Creates batches of tasks");
            //var batches = tasks.Batch(batchSize);

            //foreach (var batchOfTasks in batches)
            //{
            //    _log.LogDebug($"Executes batches of tasks");
            //    await Task.WhenAll(batchOfTasks);
            //}
        }

        protected async Task BulkInsertEntities(IEnumerable<ReceiverEntity> entities)
        {
            _log.LogDebug($"Bulk insert receiver entities");
            await _tableStorage.InsertEntityRecords<ReceiverEntity>(entities);            
        }

        public async Task<ReceiverStatusLogEnum> GetReceiverLastLogStatus(string partitionKey)
        {
            var receiverRows = await _tableStorage.GetTableEntities<ReceiverLogEntity>(partitionKey);
            var lastRow = receiverRows.OrderByDescending(x => x.RowKey).First();

            return (ReceiverStatusLogEnum)Enum.Parse(typeof(ReceiverStatusLogEnum), lastRow.Status);
        }

        public async Task<string> GetReceiverIDFromStorage(string partitionKey, string rowKey)
        {
            var receiverEntity = await _tableStorage.GetTableEntity<ReceiverEntity>(partitionKey, rowKey);
            return receiverEntity.ReceiverId;
        }

        protected async Task BulkAddLogEntryToReceivers(ReportQueueItem reportQueueItem, ReceiverStatusLogEnum statusEnum)
        {            
            SubmittalEntity submittalEntity = await _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference.ToLower(), reportQueueItem.ArchiveReference.ToLower());
            
            var partititonKey = reportQueueItem.ArchiveReference.ToLower();

            var receivers = await _tableStorage.GetTableEntities<ReceiverEntity>(partititonKey);

            var tasks = receivers.Select(s => AddToReceiverProcessLog(s.ReceiverLogPartitionKey, s.ReceiverId, statusEnum));

            await Task.WhenAll(tasks);
        }

        protected async Task UpdateEntities(IEnumerable<ReceiverEntity> entities)
        {
            await _tableStorage.UpdateEntities(entities);
        }


        protected async Task UpdateReceiverProcessStage(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverProcessStageEnum processStageEnum)
        {
            try
            {
                var receiverEntity = await _tableStorage.GetTableEntity<ReceiverEntity>(archiveReference, receiverSequenceNumber);
                receiverEntity.ProcessStage = Enum.GetName(typeof(ReceiverProcessStageEnum), processStageEnum);
                var result = _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                //_log.LogDebug($"ID={archiveReference}. Updated receiver status for receiverSequenceNumber {receiverSequenceNumber} and receiverID {receiverID}. Status: {Enum.GetName(typeof(ReceiverProcessStageEnum), processStageEnum)}.....");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"UpdateReceiverProcessStage: ArchveReference={archiveReference}. Error adding receiver record for ID={receiverSequenceNumber} and receiverID {receiverID}");
                throw;
            }
        }
        protected async Task UpdateReceiverProcessOutcome(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverProcessOutcomeEnum processOutcomeEnum)
        {
            try
            {
                var receiverEntity = await _tableStorage.GetTableEntity<ReceiverEntity>(archiveReference, receiverSequenceNumber);
                receiverEntity.ProcessOutcome = Enum.GetName(typeof(ReceiverProcessOutcomeEnum), processOutcomeEnum);
                var result = await _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                //_log.LogDebug($"ID={archiveReference}. Updated receiver status for receiverSequenceNumber {receiverSequenceNumber} and receiverID {receiverID}. Status: {Enum.GetName(typeof(ReceiverProcessStageEnum), processOutcomeEnum)}.....");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"UpdateReceiverProcessOutcome: ArchveReference={archiveReference}. Error adding receiver record for ID={receiverSequenceNumber} and receiverID {receiverID}.");
                throw;
            }
        }

        protected async Task AddToReceiverProcessLog(string receiverPartitionKey, string receiverID, ReceiverStatusLogEnum statusEnum)
        {
            try
            {
                ReceiverLogEntity receiverEntity = new ReceiverLogEntity(receiverPartitionKey, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", receiverID, statusEnum);
               // _log.LogDebug($"ID={receiverPartitionKey}. Added receiver status for {archiveReference} and receiverID {receiverID}. Status: {receiverEntity.Status}.....");
                await _tableStorage.InsertEntityRecordAsync<ReceiverLogEntity>(receiverEntity);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error adding receiver record for ID={receiverPartitionKey} and receiverID {receiverID}");
                throw;
            }
        }
    }
}
