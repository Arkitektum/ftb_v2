using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class SendQueueItem
    {
        public string ArchiveReference { get; set; }
        public string ReceiverType { get; set; }
        public string ReceiverId { get; set; }
    }
}
