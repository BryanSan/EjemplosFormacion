using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/25/overview-of-asymmetric-encryption-in-net/
    /// </summary>
    public class AsymmetricRSADecrypter : IAsymmetricRSADecrypter
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

            byte[] decryptedBytes = _publicPrivateKeyPairCipher.Value.Decrypt(cipherBytes, true);

            return decryptedBytes;
        }

        public byte[] DecryptWithFullKeyXmlBytes(string cipherString)
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentException($"{nameof(cipherString)} a desencriptar no puede estar vacio!.");

            byte[] cipherBytes = Convert.FromBase64String(cipherString);

            byte[] decryptedBytes = DecryptWithFullKeyXmlBytes(cipherBytes);

            return decryptedBytes;
        }

        public T DecryptWithFullKeyXml<T>(byte[] cipherBytes) where T : class
        {
            byte[] decryptedBytes = DecryptWithFullKeyXmlBytes(cipherBytes);

            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            T decryptedEntity = JsonConvert.DeserializeObject<T>(decryptedString);

            return decryptedEntity;
        }

        public T DecryptWithFullKeyXml<T>(string cipherString) where T : class
        {
            byte[] decryptedBytes = DecryptWithFullKeyXmlBytes(cipherString);

            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            T decryptedEntity = JsonConvert.DeserializeObject<T>(decryptedString);

            return decryptedEntity;
        }
    }
}
