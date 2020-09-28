using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FtB_Common.Interfaces;
using FtB_Common.Exceptions;

namespace FtB_Common.Storage
{
    public class TableStorage : ITableStorage
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _cloudTableClient;

        //public ILogger Logger { private get; set; }
        public TableStorage(IConfiguration configuration)
        {
            _storageAccount = CreateStorageAccountFromConnectionString(configuration["AzureStorageConnectionString"]);
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

        public async Task<TableEntity> InsertEntityRecordAsync(TableEntity entity, string tableName)
        {
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
            //Create a new storage table.
            cloudTable.CreateIfNotExists();

            try
            {
                // Create the Insert table operation
                TableOperation insertOperation = TableOperation.Insert(entity);

                // Execute the insert operation.
                TableResult result = await cloudTable.ExecuteAsync(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TableEntity InsertEntityRecord(TableEntity entity, string tableName)
        {
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
            //Create a new storage table.
            cloudTable.CreateIfNotExists();

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOperation = TableOperation.Insert(entity);

                // Execute the insert operation.
                TableResult result = cloudTable.Execute(insertOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TableEntity UpdateEntityRecord(TableEntity entity, string tableName)
        {
            try
            {
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
                // Create the InsertOrReplace table operation
                TableOperation operation = TableOperation.Replace(entity);

                // Execute the insert operation.
                TableResult result = cloudTable.Execute(operation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 412)
                {
                    //Console.WriteLine($"{ DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}: ETag={ entity.ETag }. Optimistic concurrency violation – entity has changed since it was retrieved.");
                    throw new TableStorageConcurrentException("Optimistic concurrency violation – entity has changed since it was retrieved.", 412);
                }
                else
                    throw;
            }
        }

        public T GetTableEntity<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
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
        //public async Task<T> GetTableEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity
        //{
        //    try
        //    {
        //        TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
        //        CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
        //        TableResult result = await cloudTable.ExecuteAsync(retrieveOperation);
        //        return (T)result.Result;
        //    }
        //    catch (StorageException e)
        //    {
        //        Console.WriteLine(e.Message);
        //        Console.ReadLine();
        //        throw;
        //    }
        //}

    }
}
