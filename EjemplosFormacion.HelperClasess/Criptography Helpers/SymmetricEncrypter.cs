using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.Json.Net.Abstract;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2013/11/04/symmetric-encryption-algorithms-in-net-cryptography-part-1/
    /// </summary>
    public class SymmetricEncrypter<TSymmetricAlgorithm> : ISymmetricEncrypter<TSymmetricAlgorithm>
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        readonly Lazy<TSymmetricAlgorithm> _symmetricAlgorithm;
        readonly JsonSerializerSettings _jsonSerializerSettings;

        public SymmetricEncrypter(ISymmetricAlgorithmFactory<TSymmetricAlgorithm> symmetricAlgorithmFactory, IJsonSerializerSettingsFactory jsonSerializerSettingsFactory)
        {
            // Usamos las Factories de ayuda para obtener una instancia de las clases que necesitamos para encriptar / desencriptar
            _symmetricAlgorithm = new Lazy<TSymmetricAlgorithm>(() => symmetricAlgorithmFactory.CreateSymmetricAlgorithm()); 
            _jsonSerializerSettings = jsonSerializerSettingsFactory.CreateJsonSerializerSettings();
        }

        public string Encrypt<T>(T objectToEncrypt) where T : class
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a encriptar no puede estar vacio!.");

            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateEncryptor())
            {
                // Si ya es un Json el Serializador es suficientemente inteligente para no serializarlo nuevamente
                string jsonObjectToEncrypt = JsonConvert.SerializeObject(objectToEncrypt, _jsonSerializerSettings);
                byte[] toEncryptArray = Encoding.UTF8.GetBytes(jsonObjectToEncrypt);

                // Transform the specified region of bytes array to resultArray
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                //Return the encrypted data into unreadable string format
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public T Decrypt<T>(string cipherString) where T : class
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentNullException("Mensaje a desencriptar no puede ser nulo!.");

            using (ICryptoTransform cTransform = _symmetricAlgorithm.Value.CreateDecryptor())
            {
                // Obtenemos los bytes del string encriptado
                byte[] toDecryptArray = Convert.FromBase64String(cipherString);

                // Transform the specified region of bytes array to resultArray
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                // String of Json Decrypted
                string jsonDecrypted = Encoding.UTF8.GetString(resultArray);

                // Deserialize the Json en return the entity
                return JsonConvert.DeserializeObject<T>(jsonDecrypted);
            }
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
