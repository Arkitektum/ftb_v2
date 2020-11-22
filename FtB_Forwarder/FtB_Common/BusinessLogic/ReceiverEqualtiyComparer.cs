using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FtB_Common.BusinessLogic
{
    public class ReceiverEqualtiyComparer : IEqualityComparer<ReceiverInternal>
    {
        //private readonly IDecryption _decryption;

        //public ReceiverEqualtiyComparer(IDecryptionFactory decryptionFactory)
        //{
        //    _decryption = decryptionFactory.GetDecryptor();
        //}
        public bool Equals([AllowNull] ReceiverInternal x, [AllowNull] ReceiverInternal y)
        {
            bool result = x.DecryptedId.Equals(y.DecryptedId, StringComparison.InvariantCultureIgnoreCase);
            //bool result = _decryption.DecryptText(x.Id).Equals(_decryption.DecryptText(y.Id), StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        public int GetHashCode([DisallowNull] ReceiverInternal obj)
        {
            return obj.DecryptedId.GetHashCode();
        }
    }
}
