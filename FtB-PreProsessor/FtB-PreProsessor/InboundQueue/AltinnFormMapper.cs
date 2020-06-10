using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.InboundQueue
{
    public class AltinnFormMapper
    {
        public QueuedForm MapFrom(AltinnQueuedForm altinnQueuedForm)
        {
            return new QueuedForm()
            {
                Reference = altinnQueuedForm.ArchiveReference,
                Content = new FormContent()
                {
                    ServiceCode = altinnQueuedForm.ServiceCode,
                    ServiceEditionCode = altinnQueuedForm.ServiceEditionCode.ToString()
                }
            };
        }

        public IEnumerable<QueuedForm> MapFrom(IEnumerable<AltinnQueuedForm> altinnQueuedForm)
        {
            var retVal = new List<QueuedForm>();
            foreach (var item in altinnQueuedForm)
            {
                retVal.Add(MapFrom(item));
            }

            return retVal;
        }
    }
}
