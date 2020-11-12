using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_DataModels.Mappers;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    public class SendDataProviderBase
    {
        //TODO: Rename class or file name
        protected readonly IHtmlUtils _htmlUtils;

        public SendDataProviderBase(IHtmlUtils htmlUtils)
        {
            _htmlUtils = htmlUtils;
        }

        public virtual AltinnReceiver GetReceiver(BerortPart berortPart)
        {
            var receiver = new AltinnReceiver();
            if (string.IsNullOrEmpty(berortPart.Ssn) && string.IsNullOrEmpty(berortPart.Orgnr))
                throw new System.Exception("Receiver information doesn't exist in prefill data");


            if (!string.IsNullOrEmpty(berortPart.Ssn))
            {
                receiver.Id = berortPart.Ssn;
                receiver.Type = AltinnReceiverType.Privatperson;
            }
            else
            {
                receiver.Id = berortPart.Orgnr;
                receiver.Type = AltinnReceiverType.Foretak;
            }

            return receiver;
        }
    }
}
