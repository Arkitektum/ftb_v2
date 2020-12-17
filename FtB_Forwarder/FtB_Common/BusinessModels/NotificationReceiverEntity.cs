using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class NotificationReceiverEntity : TableEntity, IStorageEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessOutcome { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string PlanId { get; set; }
        public string PlanNavn { get; set; }
        public string Reply { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverEmail { get; set; }

        public NotificationReceiverEntity() { }
        
        public NotificationReceiverEntity(string partitionKey, string rowKey, string receiverId, NotificationReceiverProcessStageEnum status, DateTime createdTimestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            ProcessStage = Enum.GetName(typeof(NotificationReceiverProcessStageEnum), status);
            CreatedTimeStamp = createdTimestamp;
        }
    }
}
