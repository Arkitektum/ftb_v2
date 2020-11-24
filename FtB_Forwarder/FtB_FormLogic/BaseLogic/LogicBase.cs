
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Ftb_Repositories;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class LogicBase<T>
    {
        protected readonly ITableStorage _tableStorage;
        protected readonly ITableStorageOperations _tableStorageOperations;
        protected readonly ILogger _log;
        protected readonly DbUnitOfWork _dbUnitOfWork;

        public LogicBase(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, DbUnitOfWork dbUnitOfWork)
        {
            _repo = repo;
            _tableStorage = tableStorage;
            _tableStorageOperations = tableStorageOperations;
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

        protected void AddReceiversProcessStatus(IEnumerable<ReceiverEntity> receivers)
        {
            Parallel.ForEach(receivers, receiver =>
            {
                _tableStorage.InsertEntityRecord<ReceiverEntity>(receiver);
            });

            //foreach (var receiver in receivers)
            //{
            //    _tableStorage.InsertEntityRecord<ReceiverEntity>(receiver);
            //}

            //bool runAgain;
            //do
            //{
            //    runAgain = false;
            //    try
            //    {
            //        _tableStorage.InsertEntityRecords<ReceiverEntity>(receivers);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //} while (runAgain);

        }

        protected void AddReceiverProcessStatus(string archiveReference, string receiverPartitionKey, string receiverID, ReceiverStatusEnum statusEnum)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    ReceiverEntity receiverEntity = new ReceiverEntity(receiverPartitionKey, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", receiverID, statusEnum, DateTime.Now);
                    _log.LogDebug($"ID={receiverPartitionKey}. Added receiver status for {archiveReference} and receiverID {receiverID}. Status: {receiverEntity.Status}.....");
                    _tableStorage.InsertEntityRecord<ReceiverEntity>(receiverEntity);
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error adding receiver record for ID={receiverPartitionKey} and receiverID {receiverID}. ArchveReference={archiveReference}. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }
    }
}
