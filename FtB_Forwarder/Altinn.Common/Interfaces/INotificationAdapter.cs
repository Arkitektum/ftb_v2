using Altinn.Common.Models;
using System.Collections.Generic;

namespace Altinn.Common.Interfaces
{
    public interface INotificationAdapter
    {
        IEnumerable<DistributionResult> SendNotification(AltinnMessageBase altinnMessage);
    }
}