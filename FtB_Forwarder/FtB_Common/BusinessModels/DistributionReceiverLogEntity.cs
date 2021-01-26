using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace FtB_Common.BusinessModels
{
    public class DistributionReceiverLogEntity : TableEntity, IStorageEntity
    {
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Status { get; set; }
        public DistributionReceiverLogEntity() { }

        public DistributionReceiverLogEntity(string partitionKey, string rowKey, string receiverId, string receiverName, DistributionReceiverStatusLogEnum status)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ReceiverId = receiverId;
            ReceiverName = receiverName;
            Status = Enum.GetName(typeof(DistributionReceiverStatusLogEnum), status);
        }
    }
}
