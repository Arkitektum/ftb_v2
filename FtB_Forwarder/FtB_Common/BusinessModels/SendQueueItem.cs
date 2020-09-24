using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class SendQueueItem: IQueueItem
    {
        public string ArchiveReference { get; set; }
        public string ReceiverType { get; set; }
        public string ReceiverId { get; set; }
    }
}
