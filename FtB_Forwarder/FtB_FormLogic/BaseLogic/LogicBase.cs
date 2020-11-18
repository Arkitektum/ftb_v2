﻿
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Ftb_Repositories;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

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
        public void LoadData(string archiveReference)
        {
            _log.LogDebug($"{GetType().Name}: LoadFormData for ArchiveReference {archiveReference}....");
            ArchiveReference = archiveReference;            
            var data = _repo.GetFormData(ArchiveReference);
            FormData = SerializeUtil.DeserializeFromString<T>(data);
        }

        protected void AddReceiverProcessStatus(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverStatusEnum statusEnum)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    ReceiverEntity receiverEntity = new ReceiverEntity($"{archiveReference}-{receiverSequenceNumber}",$"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", receiverID, statusEnum, DateTime.Now);
                    _log.LogDebug($"ID={receiverSequenceNumber}. Added receiver status for {archiveReference}, receiverID {receiverID} and {receiverSequenceNumber}. Status: {receiverEntity.Status}.....");
                    _tableStorage.InsertEntityRecord<ReceiverEntity>(receiverEntity);
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error adding receiver record for ID={receiverSequenceNumber}. ArchveReference={archiveReference}. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }
    }
}
