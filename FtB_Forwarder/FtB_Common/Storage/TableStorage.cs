﻿using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FtB_Common.Interfaces;
using FtB_Common.Exceptions;
using FtB_Common.BusinessModels;

namespace FtB_Common.Storage
{
    public class TableStorage : ITableStorage
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _cloudTableClient;

        //public ILogger Logger { private get; set; }
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
                string tableNameFromType = ""; ;
                if (typeof(T) == typeof(ReceiverEntity))
                {
                    tableNameFromType = "ftbReceivers";
                }
                else if (typeof(T) == typeof(SubmittalEntity))
                {
                    tableNameFromType = "ftbSubmittals";
                }

                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                //Create a new storage table.
                cloudTable.CreateIfNotExists();
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
        public TableEntity InsertEntityRecord<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = ""; ;
                if (typeof(T) == typeof(ReceiverEntity))
                {
                    tableNameFromType = "ftbReceivers";
                }
                else if (typeof(T) == typeof(SubmittalEntity))
                {
                    tableNameFromType = "ftbSubmittals";
                }

                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
                //Create a new storage table.
                cloudTable.CreateIfNotExists();
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

        public TableEntity UpdateEntityRecord<T>(TableEntity entity)
        {
            try
            {
                string tableNameFromType = ""; ;
                if (typeof(T) == typeof(ReceiverEntity))
                {
                    tableNameFromType = "ftbReceivers";
                }
                else if (typeof(T) == typeof(SubmittalEntity))
                {
                    tableNameFromType = "ftbSubmittals";
                }

                CloudTable cloudTable = _cloudTableClient.GetTableReference(tableNameFromType);
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
                    //Console.WriteLine($"ETag={ entity.ETag }. Optimistic concurrency violation – entity has changed since it was retrieved.");
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
                string tableNameFromType = ""; ;
                if (typeof(T) == typeof(ReceiverEntity))
                {
                    tableNameFromType = "ftbReceivers";
                }
                else if (typeof(T) == typeof(SubmittalEntity))
                {
                    tableNameFromType = "ftbSubmittals";
                }
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
