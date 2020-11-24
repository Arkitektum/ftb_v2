using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ReportQueueItem : IQueueItem
    {
        public string ArchiveReference { get; set; }
        public string ReceiverSequenceNumber { get; set; }
        public string ReceiverLogPartitionKey { get; set; }
        public Receiver Receiver { get; set; }
    }
}
