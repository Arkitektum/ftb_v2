using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distributor
{
    public class Distributor : IDistributor
    {
        public Distributor()
        {

        }

        public Task Distribute(string emailReceiverAddress, string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
