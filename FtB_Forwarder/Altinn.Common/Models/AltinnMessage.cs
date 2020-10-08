using System;
using System.Collections.Generic;

namespace Altinn.Common.Models
{

    public class AltinnMessage
    {
        public AltinnMessage()
        {
            this.Attachments = new List<Attachment>();
            this.ReplyLink = new ReplyLink();
            this.MessageData = new MessageDataType();
            this.Notifications = new List<Notification>();
        }
        public string Reportee { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }
        public AltinnReceiver Receiver { get; set; }
        public bool RespectReservable { get; set; }
        public string DistributionFormId { get; set; }
        public string ServiceOwnerCode { get; set; }
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
        public int DaysValid { get; set; }
        public DateTime? DueDate { get; set; }

        public ReplyLink ReplyLink { get; set; }

        public string PrefilledXmlDataString { get; set; }
        public MessageDataType MessageData { get; set; }
        IEnumerable<Attachment> Attachments { get; set; }
        IEnumerable<Notification> Notifications { get; set; }
        public string NotificationTemplate { get; set; }
    }
}
