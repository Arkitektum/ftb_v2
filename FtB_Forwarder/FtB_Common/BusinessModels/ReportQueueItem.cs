﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ReportQueueItem
    {
        public string ArchiveReference { get; set; }
        public List<Receiver> Receivers { get; set; }
    }
}
