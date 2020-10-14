using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common.Interfaces
{
    public interface ICorrespondenceAdapter
    {
        CorrespondenceResult SendMessage(AltinnMessageBase altinnMessage);
        CorrespondenceResult SendMessage(AltinnMessageBase altinnMessage, string externalShipmentReference);
    }
}
