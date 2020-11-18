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

        public TableStorage(IConfiguration configuration)
        {
            _storageAccount = CreateStorageAccountFromConnectionString(configuration["PrivateAzureStorageConnectionString"]);
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

        public async Task<TableEntity> InsertEntityRecordAsync<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
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

        public TableEntity InsertEntityRecord<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = GetTableName<T>();
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                cloudTable.CreateIfNotExists();
                TableOperation insertOperation = TableOperation.Insert(entity);
                TableResult result = cloudTable.Execute(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
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


        public IEnumerable<T> GetRowsFromPartitionKey<T>(string partitionKey) where T : ITableEntity, new()
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

        public IEnumerable<ReceiverEntity> GetReceivers(string partitionKey)
        {
            try
            {
                string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
                var receivers = new List<ReceiverEntity>();
                TableQuery<ReceiverEntity> tableQuery = new TableQuery<ReceiverEntity>().Where(filter);
                CloudTable cloudTable = _cloudTableClient.GetTableReference("ftbReceivers");
                var list = cloudTable.ExecuteQuery(tableQuery);
                foreach (var item in list)
                {
                    receivers.Add(item);
                }

                return receivers;
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
            if (typeof(T) == typeof(ReceiverEntity))
            {
                return "ftbReceivers";
            }
            else if (typeof(T) == typeof(SubmittalEntity))
            {
                return "ftbSubmittals";
            }
            throw new Exception("Illegal table storage name");
        }
    }
}
