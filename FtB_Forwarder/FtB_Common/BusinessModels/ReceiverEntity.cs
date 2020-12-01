using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class ReceiverEntity : TableEntity, IStorageEntity
    {
        public string ReceiverId { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessOutcome { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string ReceiverLogPartitionKey { get; set; }
        public ReceiverEntity() { }
        
        public ReceiverEntity(string partitionKey, string rowKey, string receiverId, ReceiverProcessStageEnum status, DateTime createdTimestamp, string receiverLogPartitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            ProcessStage = Enum.GetName(typeof(ReceiverProcessStageEnum), status);
            CreatedTimeStamp = createdTimestamp;
            ReceiverLogPartitionKey = receiverLogPartitionKey;
        }
    }
}
