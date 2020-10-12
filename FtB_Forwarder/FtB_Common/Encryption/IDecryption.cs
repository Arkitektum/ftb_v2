namespace FtB_Common.Encryption
{
    public interface IDecryption
    {
        string DecryptText(string cipherText);
        string EncryptText(string clearText);

    }
}