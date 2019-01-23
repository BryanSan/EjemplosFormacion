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
        readonly Lazy<RSA> _publicPrivateKeyPairCipher;

        public AsymmetricRSADecrypter(string publicPrivateKeyPair)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _publicPrivateKeyPairCipher = new Lazy<RSA>(() =>
            {
                var receiverCipher = RSA.Create();
                receiverCipher.FromXmlString(publicPrivateKeyPair);
                
                return receiverCipher;
            });
        }

        public byte[] DecryptWithFullKeyXmlBytes(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Count() <= 0) throw new ArgumentException($"{nameof(cipherBytes)} a desencriptar no puede estar vacio!.");

            byte[] decryptedBytes = _publicPrivateKeyPairCipher.Value.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA512);

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

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_publicPrivateKeyPairCipher.IsValueCreated)
                    {
                        _publicPrivateKeyPairCipher.Value.Clear();
                        _publicPrivateKeyPairCipher.Value.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
