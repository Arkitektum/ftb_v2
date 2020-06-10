using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_PreProsessor
{
    public interface IFormProcessor
    {
        Task ProcessForms(IEnumerable<QueuedForm> queuedForms);
    }
}