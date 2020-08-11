using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MetadataEnqueuer
{
    public class Enqueuer : IEnqueuer
    {
        public Enqueuer(IOptions<QueueSettings> queueSettings)
        {

        }

        public Task Enqueue(string metadataItemAsJson)
        {
            throw new NotImplementedException();
        }
    }
}
