using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Common.Storage
{
    public class TableStorageOperations : ITableStorageOperations
    {
        private readonly ITableStorage _tableStorage;

        public TableStorageOperations(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public ReceiverStatusLogEnum GetReceiverLastProcessStatus(string partitionKey)
        {
            var receiverRows = _tableStorage.GetTableEntities<ReceiverLogEntity>(partitionKey);
            var lastRow = receiverRows.OrderByDescending(x => x.RowKey).First();

            return (ReceiverStatusLogEnum)Enum.Parse(typeof(ReceiverStatusLogEnum), lastRow.Status);
        }


    }
}
