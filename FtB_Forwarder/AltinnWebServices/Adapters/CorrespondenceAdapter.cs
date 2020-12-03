using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Altinn2.Adapters.WS.Correspondence;
using AltinnWebServices.WS.Correspondence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altinn2.Adapters
{
    public class CorrespondenceAdapter : ICorrespondenceAdapter
    {
        private readonly ILogger<CorrespondenceAdapter> _logger;
        private readonly IOptions<CorrespondenceBuilderSettings> _settings;
        private readonly ICorrespondenceBuilder _correspondenceBuilder;
        private readonly ICorrespondenceClient _correspondenceClient;

        public CorrespondenceAdapter(ILogger<CorrespondenceAdapter> logger, ICorrespondenceBuilder correspondenceBuilder, ICorrespondenceClient correspondenceClient)
        {
            _logger = logger;
            _correspondenceBuilder = correspondenceBuilder;
            _correspondenceClient = correspondenceClient;
        }
        public async Task<IEnumerable<DistributionResult>> SendMessage(AltinnMessageBase altinnMessage, string externalShipmentReference)
        {
            var correspondenceResults = new List<DistributionResult>();

            _correspondenceBuilder.SetUpCorrespondence(altinnMessage.Receiver.Id, altinnMessage.ArchiveReference);

            _correspondenceBuilder.AddContent(altinnMessage.MessageData.MessageTitle, altinnMessage.MessageData.MessageSummary, altinnMessage.MessageData.MessageBody);

            //Add notification stuff if present in input
            var notificationMessage = altinnMessage as AltinnNotificationMessage;
            if (notificationMessage != null)
            {
                if (notificationMessage.Notifications.Count() > 0)
                {
                    var notification = notificationMessage.Notifications.First();
                    _correspondenceBuilder.AddEmailAndSmsNotification(NotificationEnums.NotificationCarrier.AltinnEmailPreferred,
                        "noreply@noreply.no",
                        notification.Receiver,
                        notification.EmailSubject,
                        notification.EmailContent,
                        notificationMessage.NotificationTemplate,
                        notification.SmsContent);
                }

                if (notificationMessage.ReplyLink != null)
                {
                    _correspondenceBuilder.AddReplyLink(notificationMessage.ReplyLink.Url, notificationMessage.ReplyLink.UrlTitle);
                }
            }

            // Must handle XML attachments as well
            foreach (var item in altinnMessage.Attachments)
            {
                var binaryAttachment = item as AttachmentBinary;
                if (binaryAttachment != null)
                    _correspondenceBuilder.AddBinaryAttachment(binaryAttachment.Filename, binaryAttachment.Name, binaryAttachment.BinaryContent, altinnMessage.ArchiveReference);

                var xmlAttachment = item as AttachmentXml;
                if (xmlAttachment != null)
                    _correspondenceBuilder.AddXmlFormAttachment(xmlAttachment.DataFormatId, int.Parse(xmlAttachment.DataFormatVersion), xmlAttachment.XmlStringContent, altinnMessage.ArchiveReference);
            }

            InsertCorrespondenceV2 correspondence = _correspondenceBuilder.Build();
            correspondenceResults.Add(new CorrespondenceResult() { Step = DistributionStep.PayloadCreated });

            var correspondenceResult = new CorrespondenceResult();
            try
            {
                var correspondenceResponse = await _correspondenceClient.SendCorrespondence(correspondence, externalShipmentReference);

                correspondenceResult.Message = correspondenceResponse.ReceiptText;
                if (correspondenceResponse.ReceiptStatusCode == ReceiptStatusEnum.OK)
                    correspondenceResult.Step = DistributionStep.Sent;
                else
                {
                    correspondenceResult.Step = DistributionStep.Failed;
                    correspondenceResult.Message = $"{correspondenceResult.Message} - {correspondenceResponse.ReceiptHistory}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when sending correspondence to Altinn");
                correspondenceResult.Step = DistributionStep.UnkownErrorOccurred;
                correspondenceResult.Message = $"An error occurred when sending correspondence to Altinn - {ex.Message}";
            }
            correspondenceResults.Add(correspondenceResult);

            return correspondenceResults;
        }

        public async Task<IEnumerable<DistributionResult>> SendMessage(AltinnMessageBase altinnMessage)
        {
            return await SendMessage(altinnMessage, altinnMessage.ArchiveReference);
        }
    }
}
