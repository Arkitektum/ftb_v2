using FtB_DataModels.Mappers;

namespace FtB_FormLogic
{
    public class PrefillDataProviderBase
    { 
        public virtual string GetReceiver(BerortPart berortPart)
        {
            if (string.IsNullOrEmpty(berortPart.Ssn) && string.IsNullOrEmpty(berortPart.Orgnr))
                throw new System.Exception("Receiver information doesn't exist in prefill data");

            if (!string.IsNullOrEmpty(berortPart.Ssn))
                return DecryptIfNecesarry(berortPart.Ssn);
            else
                return berortPart.Orgnr;
        }

        private string DecryptIfNecesarry(string encrypted)
        {
            return encrypted;
        }
    }
}
