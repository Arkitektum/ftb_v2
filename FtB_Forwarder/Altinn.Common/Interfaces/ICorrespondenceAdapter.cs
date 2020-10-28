using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common.Interfaces
{
    public interface ICorrespondenceAdapter
    {
        IEnumerable<DistributionResult> SendMessage(AltinnMessageBase altinnMessage);
        IEnumerable<DistributionResult> SendMessage(AltinnMessageBase altinnMessage, string externalShipmentReference);
    }
}
