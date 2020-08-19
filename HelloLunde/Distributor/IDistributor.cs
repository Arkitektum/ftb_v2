using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distributor
{
    public interface IDistributor
    {
        Task Distribute(string receiverAddress, string title, string message);
    }
}
