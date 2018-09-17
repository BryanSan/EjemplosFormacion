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

        public byte[] KeyBytes => _symmetricAlgorithm.Value.Key;
        public byte[] IVBytes => _symmetricAlgorithm.Value.IV;

        public string Key => Convert.ToBase64String(KeyBytes);
        public string IV => Convert.ToBase64String(IVBytes);

        public SymmetricService(ISymmetricAlgorithmFactory<TSymmetricAlgorithm> symmetricAlgorithmFactory, IJsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            // Usamos las Factories de ayuda para obtener una instancia de las clases que necesitamos para encriptar / desencriptar
            _symmetricAlgorithm = new Lazy<TSymmetricAlgorithm>(() => symmetricAlgorithmFactory.CreateSymmetricAlgorithm());
            _jsonSerializerSettings = jsonSerializerSettingsFactory.CreateJsonSerializerSettings();
        }

        public byte[] EncryptBytes(byte[] bytesToEncrypt)
        {
            if (bytesToEncrypt == null || bytesToEncrypt.Count() <= 0) throw new ArgumentException($"{nameof(bytesToEncrypt)} a encriptar no puede estar vacio!.");

            // Creamos el Encryptor Stream del SymmetricAlgorithm
            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateEncryptor())
            {
                // Transform the specified region of bytes array to resultArray
                byte[] encryptedBytes = cTransform.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

                return encryptedBytes;
            }
        }

        public byte[] EncryptBytes<T>(T objectToEncrypt) where T : class
        {
            if (objectToEncrypt == null) throw new ArgumentException($"{nameof(objectToEncrypt)} a encriptar no puede estar vacio!.");

            // Si ya es un Json el Serializador es suficientemente inteligente para no serializarlo nuevamente
            string jsonObjectToEncrypt = JsonConvert.SerializeObject(objectToEncrypt, _jsonSerializerSettings);

            // Obtenemos los bytes del json
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(jsonObjectToEncrypt);

            byte[] encryptedBytes = EncryptBytes(bytesToEncrypt);

            return encryptedBytes;
        }

        public string Encrypt(byte[] bytesToEncrypt)
        {
            byte[] encryptedBytes = EncryptBytes(bytesToEncrypt);

            string encryptedBytesAsBase64String = Convert.ToBase64String(encryptedBytes);

            return encryptedBytesAsBase64String;
        }

        public string Encrypt<T>(T objectToEncrypt) where T : class
        {
            byte[] encryptedBytes = EncryptBytes(objectToEncrypt);

            string encryptedBytesAsBase64String = Convert.ToBase64String(encryptedBytes);

            return encryptedBytesAsBase64String;
        }

        public byte[] DecryptBytes(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Count() <= 0) throw new ArgumentException($"{nameof(cipherBytes)} a desencriptar no puede estar vacio!.");

            // Creamos el Decryptor Stream del SymmetricAlgorithm
            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateDecryptor())
            {
                // Transform the specified region of bytes array to resultArray
                byte[] decriptedBytes = cTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                return decriptedBytes;
            }
        }

        public byte[] DecryptBytes(string cipherString)
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentException($"{nameof(cipherString)} a desencriptar no puede estar vacio!.");

            byte[] cipherBytes = Convert.FromBase64String(cipherString);

            byte[] decriptedBytes = DecryptBytes(cipherBytes);

            return decriptedBytes;
        }

        public T Decrypt<T>(byte[] cipherBytes) where T : class
        {
            byte[] decriptedBytes = DecryptBytes(cipherBytes);

            // String of Json Decrypted
            string jsonDecrypted = Encoding.UTF8.GetString(decriptedBytes);

            // Deserialize the Json en return the entity
            T deserializedEntity = JsonConvert.DeserializeObject<T>(jsonDecrypted);

            return deserializedEntity;
        }

        public T Decrypt<T>(string cipherString) where T : class
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherString);

            T deserializedEntity = Decrypt<T>(cipherBytes);

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
