using System.Collections.Generic;
using System.Linq;

namespace FtB_Common.Encryption
{
    public interface IDecryptionFactory
    {
        IDecryption GetDecryptor();
    }
    public class DecryptionFactory : IDecryptionFactory
    {
        private readonly IEnumerable<IDecryption> _decryptions;

        public DecryptionFactory(IEnumerable<IDecryption> decryptions)
        {
            _decryptions = decryptions;
        }
        public IDecryption GetDecryptor()
        {
            return _decryptions.FirstOrDefault();
        }
    }
}