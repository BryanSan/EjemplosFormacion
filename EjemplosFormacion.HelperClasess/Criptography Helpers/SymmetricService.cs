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
    /// https://dotnetcodr.com/2013/11/04/symmetric-encryption-algorithms-in-net-cryptography-part-1/
    /// </summary>
    public class SymmetricService<TSymmetricAlgorithm> : ISymmetricService<TSymmetricAlgorithm>
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        readonly Lazy<TSymmetricAlgorithm> _symmetricAlgorithm;
        readonly JsonSerializerSettings _jsonSerializerSettings;

        public string Key => Convert.ToBase64String(_symmetricAlgorithm.Value.Key);

        public string IV => Convert.ToBase64String(_symmetricAlgorithm.Value.IV);

        public SymmetricService(ISymmetricAlgorithmFactory<TSymmetricAlgorithm> symmetricAlgorithmFactory, IJsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            // Usamos las Factories de ayuda para obtener una instancia de las clases que necesitamos para encriptar / desencriptar
            _symmetricAlgorithm = new Lazy<TSymmetricAlgorithm>(() => symmetricAlgorithmFactory.CreateSymmetricAlgorithm());
            _jsonSerializerSettings = jsonSerializerSettingsFactory.CreateJsonSerializerSettings();
        }

        public byte[] EncryptBytes(byte[] bytesToEncrypt)
        {
            if (bytesToEncrypt == null || bytesToEncrypt.Count() <= 0) throw new ArgumentException($"{nameof(bytesToEncrypt)} a encriptar no puede estar vacio!.");

            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateEncryptor())
            {
                // Transform the specified region of bytes array to resultArray
                byte[] resultArray = cTransform.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

                return resultArray;
            }
        }

        public byte[] EncryptBytes<T>(T objectToEncrypt) where T : class
        {
            if (objectToEncrypt == null) throw new ArgumentException($"{nameof(objectToEncrypt)} a encriptar no puede estar vacio!.");

            // Si ya es un Json el Serializador es suficientemente inteligente para no serializarlo nuevamente
            string jsonObjectToEncrypt = JsonConvert.SerializeObject(objectToEncrypt, _jsonSerializerSettings);

            // Obtenemos los bytes del json
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(jsonObjectToEncrypt);

            byte[] encryptedBytes = EncryptBytes(toEncryptArray);

            return encryptedBytes;
        }

        public string Encrypt(byte[] bytesToEncrypt)
        {
            byte[] resultArray = EncryptBytes(bytesToEncrypt);

            string resultArrayBase64 = Convert.ToBase64String(resultArray);

            return resultArrayBase64;
        }

        public string Encrypt<T>(T objectToEncrypt) where T : class
        {
            byte[] resultArray = EncryptBytes(objectToEncrypt);

            string resultArrayBase64 = Convert.ToBase64String(resultArray);

            return resultArrayBase64;
        }

        public T Decrypt<T>(byte[] cipherBytes) where T : class
        {
            if (cipherBytes == null || cipherBytes.Count() <= 0) throw new ArgumentException($"{nameof(cipherBytes)} a desencriptar no puede estar vacio!.");

            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateDecryptor())
            {
                // Transform the specified region of bytes array to resultArray
                byte[] resultArray = cTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                // String of Json Decrypted
                string jsonDecrypted = Encoding.UTF8.GetString(resultArray);

                // Deserialize the Json en return the entity
                T deserializedEntity = JsonConvert.DeserializeObject<T>(jsonDecrypted);

                return deserializedEntity;
            }
        }

        public T Decrypt<T>(string cipherString) where T : class
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentException($"{nameof(cipherString)} a desencriptar no puede ser nulo!.");

            byte[] toDecryptArray = Convert.FromBase64String(cipherString);
            T deserializedEntity = Decrypt<T>(toDecryptArray);

            return deserializedEntity;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_symmetricAlgorithm.IsValueCreated)
                    {
                        //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                        _symmetricAlgorithm.Value.Clear();
                        _symmetricAlgorithm.Value.Dispose();
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
