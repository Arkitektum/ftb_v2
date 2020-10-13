using Altinn.Common.Models;

namespace Altinn.Common.Interfaces
{
    public interface INotificationAdapter
    {
        void SendNotification(AltinnMessageBase altinnMessage);
    }
}