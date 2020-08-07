using System;
using System.Collections.Generic;
using System.Text;

namespace Ftb_QueueRepository
{
    /// <summary>
    /// The data-structure used when queuing processing of forms
    /// </summary>
    public class MetadataQueueItem : IMetadataQueueItem
    {
        public string ServiceCode { get; set; }
        public string Reference { get; set; }
    }
}
