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
using System.Threading;

namespace FtB_Common.Storage
{
    public class TableStorage : ITableStorage
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _cloudTableClient;
        private readonly string _distributionSubmittalTable;
        private readonly string _distributionReceiverTable;
        private readonly string _distributionReceiverLogTable;
        private readonly string _notificationSenderTable;
        private readonly string _notificationSenderLogTable;

        public TableStorage(IConfiguration configuration)
        {
            _storageAccount = CreateStorageAccountFromConnectionString(configuration["PrivateAzureStorageConnectionString"]);
            _distributionSubmittalTable = configuration["TableStorage:DistributionSubmittalTable"];
            _distributionReceiverTable = configuration["TableStorage:DistributionReceiverTable"];
            _distributionReceiverLogTable = configuration["TableStorage:DistributionReceiverLogTable"];

            _notificationSenderTable = configuration["TableStorage:NotificationSenderTable"];
            _notificationSenderLogTable = configuration["TableStorage:NotificationSenderLogTable"];

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

        public TableEntity InsertEntityRecord<T>(ITableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                if (!cloudTable.Exists())
                    cloudTable.CreateIfNotExists();

                TableOperation insertOperation = TableOperation.Insert(entity);
                TableResult result = cloudTable.Execute(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TableEntity> InsertEntityRecordAsync<T>(ITableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                if (!await cloudTable.ExistsAsync())
                    await cloudTable.CreateIfNotExistsAsync();

                var insertOperation = TableOperation.InsertOrReplace(entity);
                var result = await cloudTable.ExecuteAsync(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InsertEntityRecordsAsync<T>(IEnumerable<ITableEntity> entities)
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EnsureTableExistsAsync<T>()
        {
            string tableNameFromType = GetTableName<T>();
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

            if (!await cloudTable.ExistsAsync())
                await cloudTable.CreateIfNotExistsAsync();
        }

        public async Task<bool> UpdateEntitiesAsync<T>(IEnumerable<T> entities) where T : ITableEntity
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);

                var tasks = entities.Select(async (x) => await UpdateEntityAsync(x, cloudTable));
                var success =  await Task.WhenAll(tasks);

                return success.All(x => true);

                //Parallel.ForEach(entities, entity =>
                //{
                //    TableOperation operation = TableOperation.Replace(entity);
                //    TableResult result = cloudTable.Execute(operation);
                //});

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

        private async Task<bool> UpdateEntityAsync<T>(T entity, CloudTable cloudTable) where T : ITableEntity
        {
            TableOperation operation = TableOperation.Replace(entity);
            var tableResult = await cloudTable.ExecuteAsync(operation);

            return tableResult.HttpStatusCode == 204;
        }

        public async Task<TableEntity> UpdateEntityRecordAsync<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                TableOperation operation = TableOperation.Replace(entity);
                TableResult result = await cloudTable.ExecuteAsync(operation);
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

        public async Task<T> GetTableEntityAsync<T>(string partitionKey, string rowKey) where T : ITableEntity
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                TableResult result = await cloudTable.ExecuteAsync(retrieveOperation);
                return (T)result.Result;
            }
            catch (StorageException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetTableEntitiesAsync<T>(string partitionKey) where T : ITableEntity, new()
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                var condition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
                var query = new TableQuery<T>().Where(condition);
                var entities = cloudTable.ExecuteQuery(query);

                return entities;
            }
            catch (StorageException)
            {
                throw;
            }
        }

        public IEnumerable<T> GetTableEntitiesWithStatusFilter<T>(string status) where T : ITableEntity, new()
        {
            var filters = new List<KeyValuePair<string, string>>();
            filters.Add(new KeyValuePair<string, string>("Status", status));

            return GetTableEntitiesWithFilters<T>(filters);
        }
        public IEnumerable<T> GetTableEntitiesWithFilters<T>(IEnumerable<KeyValuePair<string, string>> filter) where T : ITableEntity, new()
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                if (filter.Count() > 0)
                {
                    var finalFilter = TableQuery.GenerateFilterCondition(filter.ToList()[0].Key, QueryComparisons.Equal, filter.ToList()[0].Value);
                    for (int i = 1; i < filter.Count(); i++)
                    {
                        var conditionExtra = TableQuery.GenerateFilterCondition(filter.ToList()[i].Key, QueryComparisons.Equal, filter.ToList()[i].Value);
                        finalFilter = TableQuery.CombineFilters(finalFilter, TableOperators.And, conditionExtra);
                    }
                
                    var query = new TableQuery<T>().Where(finalFilter);
                    var lst = cloudTable.ExecuteQuery(query);

                    return lst;
                }
                throw new ArgumentOutOfRangeException("No valid arguments for filter.");
            }
            catch (StorageException)
            {
                throw;
            }
        }

        private string GetTableName<T>()
        {
            if (typeof(T) == typeof(DistributionReceiverLogEntity))
            {
                return _distributionReceiverLogTable;
            }
            else if (typeof(T) == typeof(DistributionReceiverEntity))
            {
                return _distributionReceiverTable;
            }
            else if (typeof(T) == typeof(DistributionSubmittalEntity))
            {
                return _distributionSubmittalTable;
            }
            else if (typeof(T) == typeof(NotificationSenderEntity))
            {
                return _notificationSenderTable;
            }
            else if (typeof(T) == typeof(NotificationSenderLogEntity))
            {
                return _notificationSenderLogTable;
            }

            throw new Exception("Illegal table storage name");
        }

    }
}
