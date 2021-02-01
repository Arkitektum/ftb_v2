using Altinn.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Common.Interfaces
{
    public interface IPrefillAdapter
    {
        //PrefillResult SendPrefill(AltinnDistributionMessage altinnDistributionMessage);
        Task<IEnumerable<PrefillResult>> SendPrefill(AltinnDistributionMessage altinnDistributionMessage);
    }
}