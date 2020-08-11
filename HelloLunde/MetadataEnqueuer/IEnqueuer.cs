using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataEnqueuer
{
    public interface IEnqueuer
    {
        Task Enqueue(string metadataItemAsJson);
    }
}
