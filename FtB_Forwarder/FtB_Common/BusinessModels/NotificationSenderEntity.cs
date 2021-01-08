using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class NotificationSenderEntity : TableEntity, IStorageEntity
    {
        public string InitialExternalSystemReference { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderEmail { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessOutcome { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string ReceiverId { get; set; }
        public string PlanId { get; set; }
        public string PlanNavn { get; set; }
        public string Reply { get; set; }

        public NotificationSenderEntity() { }

        public NotificationSenderEntity(string partitionKey, string rowKey, string senderId, NotificationSenderProcessStageEnum status, DateTime createdTimestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            SenderId = senderId;
            ProcessStage = Enum.GetName(typeof(NotificationSenderProcessStageEnum), status);
            CreatedTimeStamp = createdTimestamp;
        }
    }
}
