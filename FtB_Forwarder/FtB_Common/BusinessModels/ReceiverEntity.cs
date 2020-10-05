using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ReceiverEntity : TableEntity, IStorageEntity
    {
        public ReceiverEntity()
        {

        }
        public ReceiverEntity(string archiveReference, string receiverId, ReceiverStatusEnum status, DateTime createdTimestamp)
        {
            PartitionKey = archiveReference;
            RowKey = receiverId;
            Status = Enum.GetName(typeof(ReceiverStatusEnum), status);
            CreatedTimeStamp = createdTimestamp;
        }

        public ReceiverEntity(string archiveReference, string receiverId, ReceiverStatusEnum status)
        {
            PartitionKey = archiveReference;
            RowKey = receiverId;
            Status = Enum.GetName(typeof(ReceiverStatusEnum), status);
        }
        public string Status { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
    }
}
