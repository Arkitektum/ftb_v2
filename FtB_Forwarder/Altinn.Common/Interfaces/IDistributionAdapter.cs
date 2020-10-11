using Altinn.Common.Models;
using System.Collections.Generic;

namespace Altinn.Common.Interfaces
{
    public interface IDistributionAdapter
    {
        IEnumerable<AltinnDistributionResult> SendDistribution(AltinnDistributionMessage altinnMessage);
    }
}
