using Altinn.Common.Models;
using AltinnWebServices.WS.Correspondence;
using System;

namespace Altinn2.Adapters.WS.Correspondence
{
    public interface ICorrespondenceBuilder
    {
        void AddBinaryAttachment(string filename, string attachmentName, byte[] dataBytes, string sendersReference);
        void AddContent(string title, string summary, string body);
        void AddEmailAndSmsNotification(NotificationEnums.NotificationCarrier notificationCarrier, string fromEmail, string toEmail, string subject, string emailContent, string notificationTemplate, string smsContent = null);
        void AddReplyLink(string url, string urlTitle);
        void AddXmlFormAttachment(string dataFormatId, int dataFormatVersion, string xmlData, string sendersReference);
        InsertCorrespondenceV2 Build();
        void SetMessageSender(string messageSender);
        void SetUpCorrespondence(string reportee, string archiveReference);
        void SetUpCorrespondence(string reportee, string archiveReference, bool respectReservable = true);
        void SetUpCorrespondence(string reportee, string archiveReference, DateTime dueDate);
    }
}