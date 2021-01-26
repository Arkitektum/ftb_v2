﻿using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class DistributionReceiverEntity : TableEntity, IStorageEntity
    {
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessOutcome { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string ReceiverLogPartitionKey { get; set; }
        public string DistributionFormReferenceId { get; set; }
        public string SubmitPrefillTaskReceiptId { get; set; }
        //public string DistributionFormDistributionStatus { get; set; }
        public DistributionReceiverEntity() { }
        
        public DistributionReceiverEntity(string partitionKey, string rowKey, string receiverId, string receiverName, DistributionReceiverProcessStageEnum status, DateTime createdTimestamp, string receiverLogPartitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            ReceiverName = receiverName;
            ProcessStage = Enum.GetName(typeof(DistributionReceiverProcessStageEnum), status);
            CreatedTimeStamp = createdTimestamp;
            ReceiverLogPartitionKey = receiverLogPartitionKey;
        }
    }
}
