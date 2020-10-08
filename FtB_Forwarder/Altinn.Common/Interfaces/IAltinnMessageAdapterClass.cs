using Altinn.Common.Models;
using System.Collections.Generic;

namespace Altinn.Common.Interfaces
{
    public interface IAltinnMessageAdapterClass
    {
        IEnumerable<AltinnMessageResult> SendMessage(AltinnMessage altinnMessage);
    }
}
