namespace Altinn.Common.Models
{
    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Content { get; set; }
        public string Receiver { get; set; }
    }
}
