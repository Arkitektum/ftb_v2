using AltinnWebServices.WS.Prefill;
using Microsoft.Extensions.Configuration;
using System;

namespace Altinn2.Adapters.WS.Prefill
{
    public class PrefillFormTaskBuilder : IPrefillFormTaskBuilder
    {
        private PrefillFormTask _prefillForm = null;
        private readonly PrefillFormBEList _prefillFormBeList = new PrefillFormBEList();
        private readonly PrefillAttachmentBEList _prefillAttachmentBeList = new PrefillAttachmentBEList();
        private readonly NotificationBEList _notificationBeList = new NotificationBEList();
        private readonly PreFillIdentityFieldBEList _identityBeList = new PreFillIdentityFieldBEList();

        public PrefillFormTaskBuilder()
        {
        }

        public PrefillFormTask Build()
        {
            if (_prefillForm == null)
            {
                return null;
            }

            if (_prefillFormBeList.Count != 0)
            {
                _prefillForm.PreFillForms = _prefillFormBeList;
            }

            if (_prefillAttachmentBeList.Count != 0)
            {
                _prefillForm.PreFillAttachments = _prefillAttachmentBeList;
            }

            if (_notificationBeList.Count != 0)
            {
                _prefillForm.PrefillNotifications = _notificationBeList;
            }
            if (_notificationBeList.Count != 0 && _identityBeList.Count != 0)
            //if (_notificationBeList.Count != 0)
            {
                _prefillForm.PreFillIdentityFields = _identityBeList;
            }

            return _prefillForm;
        }



        public void SetupPrefillFormTask(string serviceCode, int serviceEdition, string reportee, string externalShipRef, string sendersReference, string receiversReference, int daysValid)
        {
            _prefillForm = new PrefillFormTask()
            {
                ExternalServiceCode = serviceCode,
                ExternalServiceEditionCode = serviceEdition,
                ExternalShipmentReference = externalShipRef,
                SendersReference = sendersReference,
                Reportee = reportee,
                ReceiversReference = receiversReference,
                ValidFromDate = DateTime.Now.AddDays(-1),
                ValidToDate = DateTime.Now.AddDays(daysValid),
                IsReservable = true
            };
        }


        public void AddPrefillForm(string dataFormatId, int dataFormatVersion, string formDataXml, string sendersReference)
        {
            //string formDataXmlCdata = $"<![CDATA[{formDataXml}]]>";
            string formDataXmlCdata = $"{formDataXml}";

            var prefillForm = new PrefillForm
            {
                DataFormatID = dataFormatId,
                DataFormatVersion = dataFormatVersion,
                FormDataXML = formDataXmlCdata,
                SendersReference = sendersReference,
                SignedByDefault = false,
                SigningLocked = false
            };
            _prefillFormBeList.Add(prefillForm);
        }

        public void AddPreFillIdentityField(string field, string fieldvalue)
        {
            var preFillIdentityField = new PreFillIdentityFieldBE
            {
                Index = field,
                FieldValue = fieldvalue
            };
            _identityBeList.Add(preFillIdentityField);
        }


        public void AddPrefillFormTaskAttachment(string name, string filename, byte[] attachmentData, AttachmentType attachmentType, string sendersReference)
        {
            var prefillFormTaskAttachment = new PrefillFormTaskAttachment
            {
                AttachmentData = attachmentData,
                AttachmentName = name,
                AttachmentType = attachmentType,
                SendersReference = sendersReference
            };
            _prefillAttachmentBeList.Add(prefillFormTaskAttachment);
        }


        public void ValidateNotificationEndpointsWithPrefillInstantiation()
        {
            _prefillForm.ValidateButDoNotSendNotification = true;

            // This notification should never be sent. With the flag above it will only check that there are endpoints
            // available where notifications can be sent to.

            string varslingsmal_1st_notification = "DIBK-flexvarsling-1"; // Resources.TextStrings.AltinnNotificationTemplate;
            DateTime notificaitonAtSubmitt = DateTime.Now;
            ReceiverEndPointBEList receiverEndPoints = new ReceiverEndPointBEList();

            receiverEndPoints.Add(new ReceiverEndPoint
            {
                TransportType = TransportType.EmailPreferred
            });

            var notification = new Notification
            {
                LanguageCode = "1044",
                NotificationType = varslingsmal_1st_notification,
                NotifyType = NotificationType.PreFill,
                ShipmentDateTime = notificaitonAtSubmitt,
                TextTokens = new TextTokenSubstitutionBEList()
                {
                    new TextToken()
                    {
                        TokenNum = 0,
                        TokenValue = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn" // Resources.TextStrings.AltinnNotificationMessage
                    },
                    new TextToken()
                    {
                        TokenNum = 1,
                        TokenValue = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn" //Resources.TextStrings.AltinnNotificationMessage
                    },
                    new TextToken()
                    {
                        TokenNum = 2,

                        TokenValue = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn" //Resources.TextStrings.AltinnNotificationMessage
                    },
                new TextToken()
                    {
                        TokenNum = 3,

                        TokenValue = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn" //Resources.TextStrings.AltinnNotificationMessage
                    }
            },

                ReceiverEndPoints = receiverEndPoints
            };
            _notificationBeList.Add(notification);
        }

        public void AddEmailAndSmsNotification(string fromEmail, string toEmail, string subject, string emailContent, string notificationTemplate, string smsContent = null)
        {
            //Used to be sure that the subject can be reached via altinn..
            _prefillForm.ValidateButDoNotSendNotification = true;

            ReceiverEndPointBEList receiverEndPoints = new ReceiverEndPointBEList();
            DateTime notificaitonAtSubmitt = DateTime.Now;
            string varslingsmal_1st_notification = notificationTemplate;
            string smsNotification = "Du har en melding fra Direktoratet for Byggkvalitet i Altinn"; //Resources.TextStrings.AltinnNotificationMessage;

            if (string.IsNullOrEmpty(toEmail))
            {
                receiverEndPoints.Add(new ReceiverEndPoint
                {
                    TransportType = TransportType.EmailPreferred
                });
            }
            else
            {
                receiverEndPoints.Add(new ReceiverEndPoint
                {
                    ReceiverAddress = toEmail,
                    TransportType = TransportType.Email
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

            var notification = new Notification
            {
                LanguageCode = "1044",
                NotificationType = varslingsmal_1st_notification,
                NotifyType = NotificationType.PreFill,
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

            _notificationBeList.Add(notification);
        }
    }
}
