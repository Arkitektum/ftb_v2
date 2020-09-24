using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ReportQueueItem : IQueueItem
    {
        public string ArchiveReference { get; set; }
        public List<Receiver> Receivers { get; set; }
    }
}
