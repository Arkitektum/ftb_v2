using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Interfaces
{
    public interface ITableStorage
    {
        Task<TableEntity> InsertSubmittalRecord(TableEntity tableEntity, string tableName);
        T GetTableEntity<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity;
        //Task<T> GetTableEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity;
    }
}
