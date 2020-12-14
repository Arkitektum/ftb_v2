using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IStorageEntity = FtB_Common.Interfaces.IStorageEntity;

namespace FtB_Common.BusinessModels
{
    public class NotificationSubmittalEntity : TableEntity, IStorageEntity
    {
        public string SenderId { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string Status { get; set; }
        public NotificationSubmittalEntity()
        {
        }

        public NotificationSubmittalEntity(string archiveReference, string senderId, DateTime createdTimestamp)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
            SenderId = senderId;
            Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Created);
            CreatedTimeStamp = createdTimestamp;
        }
    }
}
