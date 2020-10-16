
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace FtB_FormLogic
{
    public class LogicBase<T>
    {
        protected readonly ITableStorage _tableStorage;
        protected readonly ILogger _log;
        public LogicBase(IFormDataRepo repo, ITableStorage tableStorage, ILogger log)
        {
            _repo = repo;
            _tableStorage = tableStorage;
            _log = log;
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

        protected void UpdateReceiverEntity(string partitionKey, string rowKey, ReceiverStatusEnum status)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>(partitionKey, rowKey);
                    receiverEntity.Status = Enum.GetName(typeof(ReceiverStatusEnum), status);
                    _log.LogDebug($"ID={rowKey}. Updating changed entity for {partitionKey} and {rowKey}. Status: {receiverEntity.Status}.....");
                    var updatedEntity = _tableStorage.UpdateEntityRecord<ReceiverEntity>(receiverEntity);
                }
                catch (TableStorageConcurrentException ex)
                {
                    if (ex.HTTPStatusCode == 412)
                    {
                        int randomNumber = new Random().Next(0, 1000);
                        _log.LogInformation($"ID={rowKey}. ArchveReference={partitionKey}. Optimistic concurrency violation – entity has changed since it was retrieved. Run again after { randomNumber.ToString() } ms.");
                        Thread.Sleep(randomNumber);
                        runAgain = true;
                    }
                    else
                    {
                        _log.LogError($"Error incrementing submittal record for ID={rowKey}. ArchveReference={partitionKey}. Message: { ex.Message }");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error incrementing submittal record for ID={rowKey}. ArchveReference={partitionKey}. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }
    }
}
