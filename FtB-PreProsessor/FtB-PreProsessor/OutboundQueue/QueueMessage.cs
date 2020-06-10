using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.OutboundQueue
{
    /// <summary>
    /// The data-structure used when queuing processing of forms
    /// </summary>
    public class QueueMessage
    {
        public string Reference { get; set; }
    }
}
