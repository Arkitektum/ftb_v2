using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FtB_Common.Interfaces;

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

        public async Task<TableEntity> InsertSubmittalRecord(TableEntity entity, string tableName)
        {
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
            //Create a new storage table.
            cloudTable.CreateIfNotExists();

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the insert operation.
                TableResult result = await cloudTable.ExecuteAsync(insertOrMergeOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TableEntity> IncrementSubmittalSentCount(TableEntity entity, string tableName)
        {
            CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the insert operation.
                TableResult result = await cloudTable.ExecuteAsync(insertOrMergeOperation);
                var insertedEntity = (TableEntity)result.Result;
                return insertedEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TableResult> GetTableEntity(string tableName, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<TableEntity>(partitionKey, rowKey);
                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableName);
                TableResult result = await cloudTable.ExecuteAsync(retrieveOperation);
                return result;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

    }
}
