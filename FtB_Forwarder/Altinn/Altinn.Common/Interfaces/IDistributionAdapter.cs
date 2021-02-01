using Altinn.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Common.Interfaces
{
    public interface IDistributionAdapter
    {
        Task<IEnumerable<DistributionResult>> SendDistribution(AltinnDistributionMessage altinnMessage);
    }
}
