using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FtB_Common.Interfaces;
using FtB_Common.Exceptions;
using FtB_Common.BusinessModels;
using System.Linq;

namespace FtB_Common.Storage
{
    public class TableStorage : ITableStorage
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _cloudTableClient;
        private readonly string _submittalTable;
        private readonly string _receiverTable;
        private readonly string _receiverLogTable;

        public TableStorage(IConfiguration configuration)
        {
            _storageAccount = CreateStorageAccountFromConnectionString(configuration["PrivateAzureStorageConnectionString"]);
            _submittalTable = configuration["TableStorage:SubmittalTable"];
            _receiverTable = configuration["TableStorage:ReceiverTable"];
            _receiverLogTable = configuration["TableStorage:ReceiverLogTable"];
            _cloudTableClient = _storageAccount.CreateCloudTableClient();
        }

        public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public async Task<TableEntity> InsertEntityRecordAsync<T>(ITableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                if (!cloudTable.Exists())
                    cloudTable.CreateIfNotExists();

                TableOperation insertOperation = TableOperation.Insert(entity);
                TableResult result = await cloudTable.ExecuteAsync(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task InsertEntityRecords<T>(IEnumerable<ITableEntity> entities)
        {
            try
            {
                var batchResults = new List<TableBatchResult>();
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                if (!await cloudTable.ExistsAsync())
                    await cloudTable.CreateIfNotExistsAsync();

                var batches = entities.Batch(100);

                TableBatchOperation batchOperations;
                foreach (var batch in batches)
                {
                    batchOperations = new TableBatchOperation();

                    foreach (var entity in batch)
                    {
                        batchOperations.InsertOrReplace(entity);
                    }

                    var result = await cloudTable.ExecuteBatchAsync(batchOperations);
                    batchResults.Add(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnsureTableExists<T>()
        {
            string tableNameFromType = GetTableName<T>();
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

            if (!await cloudTable.ExistsAsync())
                await cloudTable.CreateIfNotExistsAsync();
        }

        public async Task<TableEntity> InsertEntityRecord<T>(ITableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                if (!await cloudTable.ExistsAsync())
                    await cloudTable.CreateIfNotExistsAsync();

                var insertOperation = TableOperation.Insert(entity);
                var result = await cloudTable.ExecuteAsync(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEntities<T>(IEnumerable<T> entities) where T : ITableEntity
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                Parallel.ForEach(entities, entity =>
                {
                    TableOperation operation = TableOperation.Replace(entity);
                    TableResult result = cloudTable.Execute(operation);
                });

            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 412)
                {
                    throw new TableStorageConcurrentException("Optimistic concurrency violation – entity has changed since it was retrieved.", 412);
                }
                else
                    throw;
            }
        }


        public TableEntity UpdateEntityRecord<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                TableOperation operation = TableOperation.Replace(entity);
                TableResult result = cloudTable.Execute(operation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 412)
                {
                    throw new TableStorageConcurrentException("Optimistic concurrency violation – entity has changed since it was retrieved.", 412);
                }
                else
                    throw;
            }
        }

        public T GetTableEntity<T>(string partitionKey, string rowKey) where T : ITableEntity
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                TableResult result = cloudTable.Execute(retrieveOperation);
                return (T)result.Result;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public IEnumerable<T> GetTableEntities<T>(string partitionKey) where T : ITableEntity, new()
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                var condition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
                var query = new TableQuery<T>().Where(condition);
                var lst = cloudTable.ExecuteQuery(query);

                return lst;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        private string GetTableName<T>()
        {
            if (typeof(T) == typeof(ReceiverLogEntity))
            {
                return _receiverLogTable;
            }
            else if (typeof(T) == typeof(ReceiverEntity))
            {
                return _receiverTable;
            }
            else if (typeof(T) == typeof(SubmittalEntity))
            {
                return _submittalTable;
            }
            throw new Exception("Illegal table storage name");
        }

    }

    public static class BatchExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            var nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>(batchSize);
                }
            }

            if (nextbatch.Count > 0)
            {
                yield return nextbatch;
            }
        }

    }
}
