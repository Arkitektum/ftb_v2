using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class SubmittalQueueItem : IQueueItem
    {
        public string ArchiveReference { get; set; }
    }
}
