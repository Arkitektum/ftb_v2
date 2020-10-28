using Altinn.Common.Models;
using System.Collections.Generic;

namespace Altinn.Common.Interfaces
{
    public interface IPrefillAdapter
    {
        //PrefillResult SendPrefill(AltinnDistributionMessage altinnDistributionMessage);
        IEnumerable<PrefillResult> SendPrefill(AltinnDistributionMessage altinnDistributionMessage);
    }
}