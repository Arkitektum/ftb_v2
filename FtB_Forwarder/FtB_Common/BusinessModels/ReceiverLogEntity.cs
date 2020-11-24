using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class ReceiverLogEntity : TableEntity, IStorageEntity
    {
        public string ReceiverId { get; set; }
        public string Status { get; set; }
        public ReceiverLogEntity() { }
        
        public ReceiverLogEntity(string partitionKey, string rowKey, string receiverId, ReceiverStatusLogEnum status)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            Status = Enum.GetName(typeof(ReceiverStatusLogEnum), status);
        }
    }
}
