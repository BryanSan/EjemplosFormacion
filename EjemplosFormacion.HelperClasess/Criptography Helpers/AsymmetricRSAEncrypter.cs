using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.Json.Net.Abstract;
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
    public class AsymmetricRSAEncrypter : IAsymmetricRSAEncrypter
    {
        readonly Lazy<RSA> _publicKeyCipher;
        readonly JsonSerializerSettings _jsonSerializerSettings;

        public AsymmetricRSAEncrypter(string publicKeyRsa, IJsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _publicKeyCipher = new Lazy<RSA>(() =>
            {
                var receiverCipher = RSA.Create();
                receiverCipher.FromXmlString(publicKeyRsa);
                
                return receiverCipher;
            });

            _jsonSerializerSettings = jsonSerializerSettingsFactory.CreateJsonSerializerSettings();
        }

        public byte[] EncryptWithPublicKeyXmlBytes(byte[] bytesToEncrypt)
        {
            if (bytesToEncrypt == null || bytesToEncrypt.Count() <= 0) throw new ArgumentException($"{nameof(bytesToEncrypt)} a encriptar no puede estar vacio!.");

            byte[] encryptedBytes = _publicKeyCipher.Value.Encrypt(bytesToEncrypt, RSAEncryptionPadding.OaepSHA512);
            return encryptedBytes;
        }

        public byte[] EncryptWithPublicKeyXmlBytes<T>(T objectToEncrypt) where T : class
        {
            if (objectToEncrypt == null) throw new ArgumentException($"{nameof(objectToEncrypt)} a encriptar no puede estar vacio!.");

            string jsonObjectToEncrypt = JsonConvert.SerializeObject(objectToEncrypt, _jsonSerializerSettings);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(jsonObjectToEncrypt);

            byte[] encryptedBytes = EncryptWithPublicKeyXmlBytes(toEncryptArray);

            return encryptedBytes;
        }

        public string EncryptWithPublicKeyXml(byte[] bytesToEncrypt)
        {
            byte[] encryptedBytes = EncryptWithPublicKeyXmlBytes(bytesToEncrypt);
            string encryptedStringAsBase64 = Convert.ToBase64String(encryptedBytes);

            return encryptedStringAsBase64;
        }

        public string EncryptWithPublicKeyXml<T>(T objectToEncrypt) where T : class
        {
            byte[] encryptedBytes = EncryptWithPublicKeyXmlBytes(objectToEncrypt);
            string encryptedStringAsBase64 = Convert.ToBase64String(encryptedBytes);

            return encryptedStringAsBase64;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_publicKeyCipher.IsValueCreated)
                    {
                        _publicKeyCipher.Value.Clear();
                        _publicKeyCipher.Value.Dispose();
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
