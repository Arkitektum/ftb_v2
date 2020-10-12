using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FtB_Common.Encryption
{
    public class Decryption : IDecryption
    {
        internal static RSACryptoServiceProvider PrivateKeyProvider;
        internal static RSACryptoServiceProvider PublicKeyProvider;
        private readonly IOptions<EncryptionSettings> _encryptionSettings;

        public Decryption(IOptions<EncryptionSettings> encryptionSettings)
        {
            _encryptionSettings = encryptionSettings;
        }
        private Decryption()
        {
            var certificateThumbprintSetting = _encryptionSettings.Value.CertificateThumbprint; //ConfigurationManager.AppSettings["Encrypt.CertificateThumbprint"];
            var cryptoProviders = GetRsaCryptoProviderFromKeyStore(certificateThumbprintSetting);

            PrivateKeyProvider = cryptoProviders.Item1;
            PublicKeyProvider = cryptoProviders.Item2;
        }


        public string DecryptText(string cipherText)
        {
            return DecryptString(PrivateKeyProvider, cipherText);
        }


        private static string DecryptString(RSACryptoServiceProvider rsaCryptoPrivateKey, string cipherB64Text)
        {
            var cipherBytes = Convert.FromBase64String(cipherB64Text);
            var decryptedBytes = rsaCryptoPrivateKey.Decrypt(cipherBytes, false);
            var byteConverter = new UnicodeEncoding();
            return byteConverter.GetString(decryptedBytes);
        }



        public string EncryptText(string clearText)
        {
            return EncryptString(PublicKeyProvider, clearText);
        }

        private static string EncryptString(RSACryptoServiceProvider rsaCryptoPublicKey, string clearText)
        {
            var byteConverter = new UnicodeEncoding();
            var clearTextBytes = byteConverter.GetBytes(clearText);
            var cipherBytes = rsaCryptoPublicKey.Encrypt(clearTextBytes, false);
            var clearTextBase64 = Convert.ToBase64String(cipherBytes);

            return clearTextBase64;
        }

        private static Tuple<RSACryptoServiceProvider, RSACryptoServiceProvider> GetRsaCryptoProviderFromKeyStore(string thumbprint)
        {
            X509Certificate2 cert = null;

            List<X509Store> certStores = new List<X509Store>();
            certStores.Add(new X509Store(StoreName.My, StoreLocation.CurrentUser));
            certStores.Add(new X509Store(StoreName.My, StoreLocation.LocalMachine));

            foreach (X509Store certStore in certStores)
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    thumbprint,
                    false);
                // Get the first cert with the thumbprint
                if (certCollection.Count > 0)
                {
                    cert = certCollection[0];
                }
                certStore.Close();

                // Exit foreach if already found
                if (cert != null)
                {
                    break;
                }
            }


            if (cert != null)
                return Tuple.Create((RSACryptoServiceProvider)cert.PrivateKey, (RSACryptoServiceProvider)cert.PublicKey.Key);
            throw new Exception($"No certificate found for given thumbprint {thumbprint}");
        }
    }
}