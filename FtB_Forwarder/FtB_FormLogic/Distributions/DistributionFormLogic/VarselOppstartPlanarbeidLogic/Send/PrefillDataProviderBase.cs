using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_DataModels.Mappers;

namespace FtB_FormLogic
{
    public class SendDataProviderBase
    {
        //private readonly IDecryptionFactory decryptionFactory;

        //public SendDataProviderBase(IDecryptionFactory decryptionFactory)
        //{
        //    this.decryptionFactory = decryptionFactory;
        //}
        public virtual AltinnReceiver GetReceiver(BerortPart berortPart)
        {
            var receiver = new AltinnReceiver();
            if (string.IsNullOrEmpty(berortPart.Ssn) && string.IsNullOrEmpty(berortPart.Orgnr))
                throw new System.Exception("Receiver information doesn't exist in prefill data");


            if (!string.IsNullOrEmpty(berortPart.Ssn))
            {
                receiver.Id = berortPart.Ssn; //DecryptIfNecesarry(berortPart.Ssn);
                receiver.Type = AltinnReceiverType.Privatperson;
            }
            else
            {
                receiver.Id = berortPart.Orgnr;
                receiver.Type = AltinnReceiverType.Foretak;
            }

            return receiver;
        }

        //private string DecryptIfNecesarry(string encrypted)
        //{
        //    if (encrypted.Length > 11)
        //        return decryptionFactory.GetDecryptor().DecryptText(encrypted);

        //    return encrypted;
        //}
    }
}
