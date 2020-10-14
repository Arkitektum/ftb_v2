﻿using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Altinn2.Adapters.WS.Correspondence;
using AltinnWebServices.WS.Correspondence;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Altinn2.Adapters
{
    public class CorrespondenceAdapter : ICorrespondenceAdapter
    {
        private readonly ILogger<CorrespondenceAdapter> _logger;
        private readonly ICorrespondenceBuilder _correspondenceBuilder;
        private readonly ICorrespondenceClient _correspondenceClient;

        public CorrespondenceAdapter(ILogger<CorrespondenceAdapter> logger, ICorrespondenceBuilder correspondenceBuilder, ICorrespondenceClient correspondenceClient)
        {
            _logger = logger;
            _correspondenceBuilder = correspondenceBuilder;
            _correspondenceClient = correspondenceClient;
        }
        public CorrespondenceResult SendMessage(AltinnMessageBase altinnMessage, string externalShipmentReference)
        {
            var retVal = new CorrespondenceResult();

            _correspondenceBuilder.SetUpCorrespondence(altinnMessage.Receiver.Id, altinnMessage.ArchiveReference);

            _correspondenceBuilder.AddContent(altinnMessage.MessageData.MessageTitle, altinnMessage.MessageData.MessageSummary, altinnMessage.MessageData.MessageBody);


            //Add notification stuff if present in input
            var notificationMessage = altinnMessage as AltinnNotificationMessage;
            if (notificationMessage != null && notificationMessage.Notifications.Count() > 0)
            {
                //_correspondenceBuilder.AddEmailAndSmsNotification()
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

            var correspondenceResponse = _correspondenceClient.SendCorrespondence(correspondence, externalShipmentReference);

            if (correspondenceResponse.ReceiptStatusCode != ReceiptStatusEnum.OK)
            {
                retVal.ResultType = CorrespondenceResultType.Failed;
                retVal.ResultMessage = correspondenceResponse.ReceiptText;
            }

            return retVal;
        }

        public CorrespondenceResult SendMessage(AltinnMessageBase altinnMessage)
        {
            return SendMessage(altinnMessage, altinnMessage.ArchiveReference);
        }
    }
}
