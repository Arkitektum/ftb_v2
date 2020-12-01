using FtB_Common.BusinessModels;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Interfaces
{
    public interface ITableStorage
    {
        Task<TableEntity> InsertEntityRecordAsync<T>(ITableEntity tableEntity);
        TableEntity InsertEntityRecord<T>(ITableEntity tableEntity);
        Task InsertEntityRecords<T>(IEnumerable<ITableEntity> entities);
        Task<TableEntity> UpdateEntityRecord<T>(TableEntity entity);
        Task UpdateEntities<T>(IEnumerable<T> entities) where T : ITableEntity;
        Task<T> GetTableEntity<T>(string partitionKey, string rowKey) where T : ITableEntity;
        Task<IEnumerable<T>> GetTableEntities<T>(string partitionKey) where T : ITableEntity, new();
        Task EnsureTableExists<T>();
    }
}
