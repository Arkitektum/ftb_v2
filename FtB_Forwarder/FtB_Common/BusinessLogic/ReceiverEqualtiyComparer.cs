using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FtB_Common.BusinessLogic
{
    public class ReceiverEqualtiyComparer : IEqualityComparer<ActorInternal>
    {
        //private readonly IDecryption _decryption;

        //public ReceiverEqualtiyComparer(IDecryptionFactory decryptionFactory)
        //{
        //    _decryption = decryptionFactory.GetDecryptor();
        //}
        public bool Equals([AllowNull] ActorInternal x, [AllowNull] ActorInternal y)
        {
            bool result = x.DecryptedId.Equals(y.DecryptedId, StringComparison.InvariantCultureIgnoreCase)
                            && x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        public int GetHashCode([DisallowNull] ActorInternal obj)
        {
            return obj.DecryptedId.GetHashCode();
        }
    }
}
