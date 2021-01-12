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
        Task InsertEntityRecordsAsync<T>(IEnumerable<ITableEntity> entities);
        Task<TableEntity> UpdateEntityRecordAsync<T>(TableEntity entity) where T : ITableEntity;
        Task<bool> UpdateEntitiesAsync<T>(IEnumerable<T> entities) where T : ITableEntity;
        Task<T> GetTableEntityAsync<T>(string partitionKey, string rowKey) where T : ITableEntity;
        Task<IEnumerable<T>> GetTableEntitiesAsync<T>(string partitionKey) where T : ITableEntity, new();
        Task EnsureTableExistsAsync<T>();
        IEnumerable<T> GetTableEntitiesWithStatusFilter<T>(string status) where T : ITableEntity, new();
        IEnumerable<T> GetTableEntitiesWithFilters<T>(IEnumerable<KeyValuePair<string, string>> filter) where T : ITableEntity, new();
    }
}
