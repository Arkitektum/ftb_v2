﻿using System;
using System.Collections.Generic;

namespace Altinn.Common.Models
{
    public abstract class AltinnMessageBase
    {
        public AltinnMessageBase()
        {
            this.Attachments = new List<Attachment>();
            this.MessageData = new MessageDataType();
            this.Receiver = new AltinnReceiver();
        }
        public MessageDataType MessageData { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public AltinnReceiver Receiver { get; set; }
        public string ArchiveReference { get; set; }
        public bool RespectReservable { get; set; } = true;    
    }

    public class AltinnMessage : AltinnMessageBase
    {}

    public class AltinnNotificationMessage : AltinnMessageBase
    {
        public AltinnNotificationMessage()
        {
            this.Notifications = new List<Notification>();
        }

        public IEnumerable<Notification> Notifications { get; set; }
        public string NotificationTemplate { get; set; }
    }

    public class AltinnDistributionMessage
    {
        public AltinnDistributionMessage()
        {   
            this.ReplyLink = new ReplyLink();
            this.NotificationMessage = new AltinnNotificationMessage();
        }        

        public AltinnNotificationMessage NotificationMessage { get; set; }
        public string PrefillDataFormatId { get; set; }
        public string PrefillDataFormatVersion { get; set; }
        public string PrefillServiceCode { get; set; }
        public string PrefillServiceEditionCode { get; set; }
        public string PrefilledXmlDataString { get; set; }

        public string DistributionFormReferenceId { get; set; }
        public int DaysValid { get; set; }
        public DateTime? DueDate { get; set; }
        public ReplyLink ReplyLink { get; set; }
    }
}