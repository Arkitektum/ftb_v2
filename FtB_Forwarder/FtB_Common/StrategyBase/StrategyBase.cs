using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FtB_Common
{
    public abstract class StrategyBase
    {
        private IFormLogic _formLogic;
        public IFormLogic FormLogicBeingProcessed { get => _formLogic; set { _formLogic = value; ArchiveReference = value.ArchiveReference; Receivers = value.Receivers; } }
        private readonly ITableStorage _tableStorage;
        private readonly ILogger _log;
        protected string ArchiveReference;
        protected List<Receiver> Receivers;
               
        public StrategyBase(ITableStorage tableStorage, ILogger log)
        {
            //FormLogicBeingProcessed = formLogic;
            _tableStorage = tableStorage;
            _log = log;
            //ArchiveReference = formLogic.ArchiveReference;
            //Receivers = formLogic.Receivers;
        }

        //public ReceiverEntity GetReceiverEntityWithLegalStoreageCharacters(string archiveReference, string receiverId)
        //{
        //    return new ReceiverEntity(archiveReference, receiverId.Replace("/",""));
        //}

        protected void UpdateReceiverEntity(ReceiverEntity entity)
        {
            bool runAgain;
            do
            {
                runAgain = false;
                try
                {
                    string receiverIdWithLegalStoreageCharacters = entity.RowKey.Replace("/", "");
                    ReceiverEntity receiverEntity = _tableStorage.GetTableEntity<ReceiverEntity>("ftbReceivers", entity.PartitionKey, receiverIdWithLegalStoreageCharacters);
                    _log.LogInformation($"ID={entity.RowKey}. Before ReceiverEntity update for archiveRefrrence {entity.PartitionKey}. Status: {entity.Status}. ETag: {entity.ETag}");
                    receiverEntity.Status = entity.Status;

                    //Log the record to be inserted
                    _log.LogDebug($"ID={entity.RowKey}. After changed entity property for {entity.PartitionKey}. Status: {entity.Status}. ETag: {entity.ETag}");
                    var updatedEntity = _tableStorage.UpdateEntityRecord(receiverEntity, "ftbReceivers");
                    _log.LogDebug($"ID={entity.RowKey}. After update for {entity.PartitionKey}. Status: {entity.Status}. ETag: {entity.ETag}");
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
