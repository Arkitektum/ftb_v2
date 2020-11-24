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
    public class SubmittalEntity : TableEntity, IStorageEntity
    {
        public int ReceiverCount { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string Status { get; set; }
        public SubmittalEntity()
        {
        }

        public SubmittalEntity(string archiveReference, int receiverCount, DateTime createdTimestamp)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
            ReceiverCount = receiverCount;
            Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Created);
            CreatedTimeStamp = createdTimestamp;
        }

        public SubmittalEntity(string archiveReference, SubmittalStatusEnum status)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
            Status = Enum.GetName(typeof(SubmittalStatusEnum), status);
        }
        public SubmittalEntity(string archiveReference)
        {
            PartitionKey = archiveReference;
            RowKey = archiveReference;
        }



    }
}
