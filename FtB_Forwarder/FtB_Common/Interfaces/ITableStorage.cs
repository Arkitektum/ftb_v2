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
        Task<TableResult> GetTableEntity(string tableName, string partitionKey, string rowKey);
    }
}
