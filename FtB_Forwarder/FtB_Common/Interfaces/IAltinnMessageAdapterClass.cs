using Altinn.Common;
using FtB_Common.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{

    public class AltinnMessageResult
    {
        public AltinnMessageStatus Status { get; set; }
    }

    public enum AltinnMessageStatus
    {
        PrefillCreated,
        PrefillSent,
        MessageCreated,
        MessageSent
    }

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
        public Receiver Receiver { get; set; }
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
    public class ReplyLink
    {
        public string Url { get; set; }
        public string UrlTitle { get; set; }
    }
    public class MessageDataType
    {
        public string MessageTitle { get; set; }
        public string MessageSummary { get; set; }
        public string MessageBody { get; set; }
    }

    public class Attachment
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public string SendersReference { get; set; }
        public byte[] Bytes { get; set; }
        public string Url { get; set; }
    }

    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Content { get; set; }
        public string Receiver { get; set; }
    }

    public enum NotificationType
    {
        Sms,
        Email
    }

    public interface IAltinnMessageAdapterClass
    {
        IEnumerable<AltinnMessageResult> SendMessage(AltinnMessage altinnMessage);
    }
}
