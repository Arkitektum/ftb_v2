using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_DataModels.Mappers;

namespace FtB_FormLogic
{
    public class PrefillDataProviderBase
    { 
        public virtual AltinnReceiver GetReceiver(BerortPart berortPart)
        {
            var receiver = new AltinnReceiver();
            if (string.IsNullOrEmpty(berortPart.Ssn) && string.IsNullOrEmpty(berortPart.Orgnr))
                throw new System.Exception("Receiver information doesn't exist in prefill data");


            if (!string.IsNullOrEmpty(berortPart.Ssn))
            {
                receiver.Id = DecryptIfNecesarry(berortPart.Ssn);
                receiver.Type = AltinnReceiverType.Privatperson;
            }
            else
            {
                receiver.Id = berortPart.Orgnr;
                receiver.Type = AltinnReceiverType.Foretak;
            }

            return receiver;
        }

        private string DecryptIfNecesarry(string encrypted)
        {
            return encrypted;
        }
    }
}
