using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class NotificationSenderLogEntity : TableEntity, IStorageEntity
    {
        public string SenderId { get; set; }
        public string Status { get; set; }
        public NotificationSenderLogEntity() { }
        
        public NotificationSenderLogEntity(string partitionKey, string rowKey, string senderId, NotificationSenderStatusLogEnum status)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            SenderId = senderId;
            Status = Enum.GetName(typeof(NotificationSenderStatusLogEnum), status);
        }
    }
}
