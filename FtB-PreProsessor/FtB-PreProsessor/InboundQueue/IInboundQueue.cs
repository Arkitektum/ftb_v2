using System.Collections.Generic;

namespace FtB_PreProsessor.InboundQueue
{
    public interface IInboundQueue
    {
        IEnumerable<QueuedForm> GetQueuedFormsFor(List<string> serviceCodes);
    }
}