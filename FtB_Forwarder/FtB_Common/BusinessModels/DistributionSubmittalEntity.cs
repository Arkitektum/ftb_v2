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
    public class DistributionSubmittalEntity : TableEntity, IStorageEntity
    {
        public string SenderId { get; set; }
        public int ReceiverCount { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string Status { get; set; }
        public DistributionSubmittalEntity()
        {
        }

        public DistributionSubmittalEntity(string archiveReference, string senderId, int receiverCount, DateTime createdTimestamp)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
            SenderId = senderId;
            ReceiverCount = receiverCount;
            Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Created);
            CreatedTimeStamp = createdTimestamp;
        }

        public DistributionSubmittalEntity(string archiveReference, SubmittalStatusEnum status)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
            Status = Enum.GetName(typeof(SubmittalStatusEnum), status);
        }
        public DistributionSubmittalEntity(string archiveReference)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
        }



    }
}
