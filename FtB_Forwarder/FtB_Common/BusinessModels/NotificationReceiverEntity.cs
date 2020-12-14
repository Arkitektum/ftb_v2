using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class NotificationReceiverEntity : TableEntity, IStorageEntity
    {
        public string ReceiverId { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessOutcome { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public NotificationReceiverEntity() { }
        
        public NotificationReceiverEntity(string partitionKey, string rowKey, string receiverId, ReceiverProcessStageEnum status, DateTime createdTimestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            ProcessStage = Enum.GetName(typeof(ReceiverProcessStageEnum), status);
            CreatedTimeStamp = createdTimestamp;
        }
    }
}
