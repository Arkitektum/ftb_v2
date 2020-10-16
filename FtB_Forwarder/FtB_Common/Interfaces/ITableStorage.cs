using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Interfaces
{
    public interface ITableStorage
    {
        Task<TableEntity> InsertEntityRecordAsync<T>(TableEntity tableEntity);
        TableEntity InsertEntityRecord<T>(TableEntity tableEntity);
        TableEntity UpdateEntityRecord<T>(TableEntity entity);
        T GetTableEntity<T>(string partitionKey, string rowKey) where T : ITableEntity;
        //Task<T> GetTableEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity;
    }
}
