using Altinn.Common.Models;
using AltinnWebServices.WS.Correspondence;
using Microsoft.Extensions.Options;
using System;

namespace Altinn2.Adapters.WS.Correspondence
{
    public class CorrespondenceBuilder : ICorrespondenceBuilder
    {
        private InsertCorrespondenceV2 _correspondence = null;
        private readonly ExternalContentV2 _content = new ExternalContentV2();
        private readonly AttachmentsV2 _attachments = new AttachmentsV2();

        private readonly BinaryAttachmentExternalBEV2List
            _binaryAttachmentList = new BinaryAttachmentExternalBEV2List();

        private readonly XmlAttachmentListV2 _xmlAttachmentList = new XmlAttachmentListV2();
        private readonly NotificationBEList _notificationList = new NotificationBEList();
        private readonly IOptions<CorrespondenceSettings> _correspondenceSettings;

        public CorrespondenceBuilder(IOptions<CorrespondenceSettings> correspondenceSettings)
        {
            _correspondenceSettings = correspondenceSettings;
        }

        public InsertCorrespondenceV2 Build()
        {
            if (_correspondence == null)
            {
                return null;
            }

            if (_xmlAttachmentList.Count != 0)
            {
                _attachments.XmlAttachmentList = _xmlAttachmentList;
            }

            if (_binaryAttachmentList.Count != 0)
            {
                _attachments.BinaryAttachments = _binaryAttachmentList;
            }

            if (_binaryAttachmentList.Count != 0 || _xmlAttachmentList.Count != 0)
            {
                _content.Attachments = _attachments;
                _correspondence.Content = _content;
            }
            else if (!string.IsNullOrWhiteSpace(_content.LanguageCode))
            {
                _correspondence.Content = _content;
            }

            if (_notificationList.Count != 0)
            {
                _correspondence.Notifications = _notificationList;
            }

            return _correspondence;
        }

        public void AddEmailAndSmsNotification(NotificationEnums.NotificationCarrier notificationCarrier, string fromEmail, string toEmail, string subject, string emailContent, string notificationTemplate, string smsContent = null)
        {
            ReceiverEndPointBEList receiverEndPoints = new ReceiverEndPointBEList();
            DateTime notificaitonAtSubmitt = DateTime.Now;
            var varslingsmal_1st_notification = notificationTemplate ?? "DIBK-flexvarsling-1"; //Resources.TextStrings.AltinnNotificationTemplate;

            string smsNotification = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn"; // Resources.TextStrings.AltinnNotificationMessage;


            // Send to a supplied email address
            if (notificationCarrier == NotificationEnums.NotificationCarrier.EmailFromDistribution)
            {
                if (IsValidEmailAddress(toEmail))
                {
                    receiverEndPoints.Add(new ReceiverEndPoint
                    {
                        ReceiverAddress = toEmail,
                        TransportType = TransportType.Email
                    });
                }
            }

            // Send to a supplied email or through Altinn's notificaton system if invalid e-mail address
            if (notificationCarrier == NotificationEnums.NotificationCarrier.EmailFromDistributionOrAltinnWhenInvalidEmailAddress)
            {
                if (IsValidEmailAddress(toEmail))
                {
                    receiverEndPoints.Add(new ReceiverEndPoint
                    {
                        ReceiverAddress = toEmail,
                        TransportType = TransportType.Email
                    });
                }
                else
                {
                    receiverEndPoints.Add(new ReceiverEndPoint
                    {
                        TransportType = TransportType.EmailPreferred
                    });
                }
            }




            // Use Altinn's built-in notification system with email preferred
            if (notificationCarrier == NotificationEnums.NotificationCarrier.Altinn ||
                notificationCarrier == NotificationEnums.NotificationCarrier.AltinnEmailPreferred)
            {
                receiverEndPoints.Add(new ReceiverEndPoint
                {
                    TransportType = TransportType.EmailPreferred
                });
            }

            // Use Altinn's built-in notification system with email preferred
            if (notificationCarrier == NotificationEnums.NotificationCarrier.AltinnSmsPreferred)
            {
                receiverEndPoints.Add(new ReceiverEndPoint
                {
                    TransportType = TransportType.SMSPreferred
                });
            }

            if (!string.IsNullOrEmpty(smsContent))
            {
                smsNotification = smsContent;
            }

            var emailContentLength = emailContent.Length;
            var emailContentPartOne = emailContent;
            var emailContentPartTwo = "";
            int messageMaxLength = 900;
            if (emailContentLength > messageMaxLength)
            {
                emailContentPartOne = emailContent.Substring(0, messageMaxLength);
                emailContentPartTwo = emailContent.Substring(messageMaxLength);
            }


            var notification = new Notification1
            {
                LanguageCode = "1044",
                NotificationType = varslingsmal_1st_notification,
                ShipmentDateTime = notificaitonAtSubmitt,
                TextTokens = new TextTokenSubstitutionBEList()
                {
                    new TextToken()
                    {
                        TokenNum = 0,
                        TokenValue = smsNotification
                    },
                    new TextToken()
                    {
                        TokenNum = 1,
                        TokenValue = subject
                    },
                    new TextToken()
                    {
                        TokenNum = 2,

                        TokenValue = emailContentPartOne
                    },
                    new TextToken()
                    {
                        TokenNum = 3,

                        TokenValue = emailContentPartTwo
                    }
                },
                ReceiverEndPoints = receiverEndPoints
            };
            _notificationList.Add(notification);
        }


        /// <summary>
        /// Bygger melding med frist
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="serviceCodeEdition"></param>
        /// <param name="reportee"></param>
        /// <param name="archiveReference"></param>
        /// <param name="dueDate"></param>
        public void SetUpCorrespondence(string reportee, string archiveReference, DateTime dueDate)
        {
            _correspondence = new InsertCorrespondenceV2
            {
                ServiceCode = _correspondenceSettings.Value.ServiceCode,
                ServiceEdition = _correspondenceSettings.Value.ServiceCodeEdition,
                Reportee = reportee,
                ArchiveReference = archiveReference,
                AllowForwarding = true,
                CaseID = null,
                DueDateTime = dueDate,
                VisibleDateTime = DateTime.Now.AddDays(-1),
                IsReservable = true
            };
        }

        /// <summary>
        /// Bygger melding uten frist
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="serviceCodeEdition"></param>
        /// <param name="reportee"></param>
        /// <param name="archiveReference"></param>
        public void SetUpCorrespondence(string reportee, string archiveReference)
        {
            _correspondence = new InsertCorrespondenceV2
            {
                ServiceCode = _correspondenceSettings.Value.ServiceCode,
                ServiceEdition = _correspondenceSettings.Value.ServiceCodeEdition,
                Reportee = reportee,
                ArchiveReference = archiveReference,
                AllowForwarding = true,
                CaseID = null,
                VisibleDateTime = DateTime.Now.AddDays(-1),
                IsReservable = true
            };
        }
        public void SetUpCorrespondence(string reportee, string archiveReference, bool respectReservable = true)
        {
            _correspondence = new InsertCorrespondenceV2
            {
                ServiceCode = _correspondenceSettings.Value.ServiceCode,
                ServiceEdition = _correspondenceSettings.Value.ServiceCodeEdition,
                Reportee = reportee,
                ArchiveReference = archiveReference,
                AllowForwarding = true,
                CaseID = null,
                VisibleDateTime = DateTime.Now.AddDays(-1),
                IsReservable = respectReservable
            };
        }

        public void SetMessageSender(string messageSender)
        {
            _correspondence.MessageSender = messageSender;
        }
        public void AddContent(string title, string summary, string body)
        {
            _content.LanguageCode = "1044";
            _content.MessageTitle = title;
            _content.MessageSummary = summary;
            _content.MessageBody = body;

        }


        public void AddXmlFormAttachment(string dataFormatId, int dataFormatVersion, string xmlData, string sendersReference)
        {

            XmlAttachmentV2 xmlAttachment = new XmlAttachmentV2
            {
                DataFormatId = dataFormatId,
                DataFormatVersion = dataFormatVersion,
                FormDataXml = $"{xmlData}",
                SendersReference = sendersReference
            };
            _xmlAttachmentList.Add(xmlAttachment);
        }


        public void AddBinaryAttachment(string filename, string attachmentName, byte[] dataBytes, string sendersReference)
        {
            BinaryAttachmentV2 binaryAttachment = new BinaryAttachmentV2
            {
                FileName = filename,
                Name = attachmentName + FilenameWithBrackets(filename),
                Encrypted = false,
                Data = dataBytes,
                SendersReference = sendersReference,
                FunctionType = AttachmentFunctionType.Unspecified
            };

            _binaryAttachmentList.Add(binaryAttachment);
        }

        private string FilenameWithBrackets(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                return " (" + filename + ")";
            }

            return "";
        }


        public void AddReplyLink(string url, string urlTitle)
        {
            if (_correspondence.ReplyOptions == null) _correspondence.ReplyOptions = new CorrespondenceInsertLinkBEList();
            _correspondence.ReplyOptions.Add(new ReplyOption() { URL = new InsertCorrespondenceLinkServiceURL() { LinkURL = url, LinkText = urlTitle } });

        }

        private static bool IsValidEmailAddress(string email)
        {
            // https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}