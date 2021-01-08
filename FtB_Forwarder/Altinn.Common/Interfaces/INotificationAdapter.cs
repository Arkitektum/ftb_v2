using Altinn.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Common.Interfaces
{
    public interface INotificationAdapter
    {
        Task<IEnumerable<DistributionResult>> SendNotificationAsync(AltinnMessageBase altinnMessage);
    }
}