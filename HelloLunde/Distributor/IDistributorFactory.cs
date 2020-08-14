using System;
using System.Collections.Generic;
using System.Text;

namespace Distributor
{
    public interface IDistributorFactory
    {
        IDistributor GetDistributor();
    }
}
