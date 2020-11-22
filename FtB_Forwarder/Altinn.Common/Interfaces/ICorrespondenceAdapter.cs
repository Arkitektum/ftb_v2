using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.Common.Interfaces
{
    public interface ICorrespondenceAdapter
    {
        Task<IEnumerable<DistributionResult>> SendMessage(AltinnMessageBase altinnMessage);
        Task<IEnumerable<DistributionResult>> SendMessage(AltinnMessageBase altinnMessage, string externalShipmentReference);
    }
}
