using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common.Interfaces
{
    public interface ICorrespondenceAdapter
    {
        void SendMessage(AltinnDistributionMessage altinnMessage, string prefillReference);
    }
}
