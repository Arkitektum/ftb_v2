using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ReceiverEntity : TableEntity
    {
        public ReceiverEntity()
        {

        }
        public ReceiverEntity(string archiveReference, string receiverId)
        {
            this.PartitionKey = archiveReference;
            this.RowKey = receiverId;
        }
        public string Status { get; set; }
    }
}
