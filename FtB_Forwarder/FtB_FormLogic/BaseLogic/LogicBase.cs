
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
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

        protected void UpdateReceiverEntity(ReceiverEntity entity)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>("ftbReceivers", entity.PartitionKey, entity.RowKey);
                    _log.LogTrace($"ID={entity.RowKey}. Before ReceiverEntity update for archiveRefrrence {entity.PartitionKey}. Status: {entity.Status}.");
                    receiverEntity.Status = entity.Status;

                    //Log the record to be inserted
                    _log.LogDebug($"ID={entity.RowKey}. Updating changed entity for {entity.PartitionKey} and {entity.RowKey}. Status: {entity.Status}.....");
                    var updatedEntity = _tableStorage.UpdateEntityRecord(receiverEntity, "ftbReceivers");
                }
                catch (TableStorageConcurrentException ex)
                {
                    if (ex.HTTPStatusCode == 412)
                    {
                        int randomNumber = new Random().Next(0, 1000);
                        _log.LogInformation($"ID={entity.RowKey}. ArchveReference={entity.PartitionKey}. Optimistic concurrency violation – entity has changed since it was retrieved. Run again after { randomNumber.ToString() } ms.");
                        Thread.Sleep(randomNumber);
                        runAgain = true;
                    }
                    else
                    {
                        _log.LogError($"Error incrementing submittal record for ID={entity.RowKey}. ArchveReference={entity.PartitionKey}. Message: { ex.Message }");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Error incrementing submittal record for ID={entity.RowKey}. ArchveReference={entity.PartitionKey}. Message: { ex.Message }");
                    throw ex;
                }
            } while (runAgain);
        }

    }
}
