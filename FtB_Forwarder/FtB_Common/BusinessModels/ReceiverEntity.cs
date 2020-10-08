﻿using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class ReceiverEntity : TableEntity, IStorageEntity
    {
        public ReceiverEntity() { }
        
        public ReceiverEntity(string archiveReference, string storageRowKey, string receiverId, ReceiverStatusEnum status, DateTime createdTimestamp)
        {
            PartitionKey = archiveReference;
            RowKey = storageRowKey;
            ReceiverId = receiverId;
            Status = Enum.GetName(typeof(ReceiverStatusEnum), status);
            CreatedTimeStamp = createdTimestamp;
        }

        public ReceiverEntity(string archiveReference, string storageRowKey, ReceiverStatusEnum status)
        {
            PartitionKey = archiveReference;
            RowKey = storageRowKey;
            Status = Enum.GetName(typeof(ReceiverStatusEnum), status);
        }
        public ReceiverEntity(string archiveReference, string storageRowKey)
        {
            PartitionKey = archiveReference;
            RowKey = storageRowKey;
        }
        private string ReceiverId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
    }
}
