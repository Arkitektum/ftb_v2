using FtB_Common.BusinessModels;
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
        public ReceiverStatusEnum GetReceiverLastProcessStatus(string partitionKey)
        {
            var receiverRows = _tableStorage.GetRowsFromPartitionKey<ReceiverEntity>(partitionKey);
            var lastRow = receiverRows.OrderByDescending(x => x.RowKey).First();

            return (ReceiverStatusEnum)Enum.Parse(typeof(ReceiverStatusEnum), lastRow.Status);
        }

        public bool SuccessfullySentToReceiver(string partitionKey)
        {
            var receiverRows = _tableStorage.GetRowsFromPartitionKey<ReceiverEntity>(partitionKey);

            return receiverRows.Any(x => x.Status.Equals(Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.CorrespondenceSent)));
        }
    }
}
