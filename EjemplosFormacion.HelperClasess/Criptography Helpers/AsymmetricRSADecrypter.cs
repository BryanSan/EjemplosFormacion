using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/25/overview-of-asymmetric-encryption-in-net/
    /// </summary>
    public class AsymmetricRSADecrypter
    {
        readonly Lazy<RSACryptoServiceProvider> _publicPrivateKeyPairCipher;

        public AsymmetricRSADecrypter(string publicPrivateKeyPair)
        {
            _publicPrivateKeyPairCipher = new Lazy<RSACryptoServiceProvider>(() =>
            {
                var receiverCipher = new RSACryptoServiceProvider();
                receiverCipher.FromXmlString(publicPrivateKeyPair);

                return receiverCipher;
            });
        }

        public byte[] DecryptWithFullKeyXmlBytes(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Count() <= 0) throw new ArgumentException($"{nameof(cipherBytes)} a desencriptar no puede estar vacio!.");

            byte[] decryptBytes = _publicPrivateKeyPairCipher.Value.Decrypt(cipherBytes, true);

            return decryptBytes;
        }

        public byte[] DecryptWithFullKeyXmlBytes(string cipherString)
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentException($"{nameof(cipherString)} a desencriptar no puede estar vacio!.");

            byte[] cipherBytes = Convert.FromBase64String(cipherString);

            byte[] decryptBytes = DecryptWithFullKeyXmlBytes(cipherBytes);

            return decryptBytes;
        }

        public string DecryptWithFullKeyXml(byte[] cipherBytes)
        {
            byte[] decryptBytes = DecryptWithFullKeyXmlBytes(cipherBytes);

            string decryptBytesBase64 = Encoding.UTF8.GetString(decryptBytes);

            return decryptBytesBase64;
        }

        public string DecryptWithFullKeyXml(string cipherString)
        {
            byte[] decryptBytes = DecryptWithFullKeyXmlBytes(cipherString);

            string decryptBytesBase64 = Encoding.UTF8.GetString(decryptBytes);

            return decryptBytesBase64;
        }
    }
}
