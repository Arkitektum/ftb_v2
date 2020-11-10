namespace Altinn.Common.Models
{
    public class Notification
    {
        public NotificationType Type { get; set; }
        public string EmailContent { get; set; }
        public string SmsContent { get; set; }
        public string Receiver { get; set; }
    }
}
