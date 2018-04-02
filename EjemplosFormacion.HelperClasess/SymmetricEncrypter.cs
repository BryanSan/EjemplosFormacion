using EjemplosFormacion.HelperClasess.Abstract;
using EjemplosFormacion.HelperClasess.Json.Net;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess
{
    public class SymmetricEncrypter<TCrypt, THasher> : ISymmetricEncrypter<TCrypt, THasher>
        where TCrypt : SymmetricAlgorithm, new()
        where THasher : HashAlgorithm, new()
    {
        //@"Fs2y9EM9vYpJwD8DpgNbu+3KfOQjelaXowfTNsDYLgU="
        const string CryptoKey = @"Fs2y9EM9vYpJwD8DpgNbu+3KfOQjelaXowfTNsDYLgU=";

        // @"XezL35y3vQrxUkqXf4SBPQ=="
        const string CryptoIVKey = @"XezL35y3vQrxUkqXf4SBPQ==";

        static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new WritablePropertiesOnlyResolver(),
        };

        public string Encrypt<T>(T objectToEncrypt, bool useHashing = true)
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a encriptar no puede estar vacio!.");

            // Si ya es un Json el Serializador es suficientemente inteligente para no serializarlo nuevamente
            string toEncrypt = JsonConvert.SerializeObject(objectToEncrypt, settings);
            byte[] cryptoKeyArray;
            byte[] cryptoIVKeyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            BuildKeysAndIV(out cryptoKeyArray, out cryptoIVKeyArray, useHashing);

            using (TCrypt cryptoSymmetricProvider = new TCrypt())
            {
                // Configure SymmetricProvider Key, IVKey, Mode and Padding
                ConfigureCryptoSymmetricProvider(cryptoKeyArray, cryptoIVKeyArray, cryptoSymmetricProvider);

                using (ICryptoTransform cTransform = cryptoSymmetricProvider.CreateEncryptor())
                {
                    //transform the specified region of bytes array to resultArray
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    //Release resources held by AesCng Encryptor
                    cryptoSymmetricProvider.Clear();

                    //Return the encrypted data into unreadable string format
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }

        public T Decrypt<T>(string cipherString, bool useHashing = true)
        {
            if (string.IsNullOrWhiteSpace(cipherString)) throw new ArgumentNullException("Mensaje a desencriptar no puede ser nulo!.");

            byte[] cryptoKeyArray;
            byte[] cryptoIVKeyArray;
            byte[] toDecryptArray = Convert.FromBase64String(cipherString);

            BuildKeysAndIV(out cryptoKeyArray, out cryptoIVKeyArray, useHashing);

            using (TCrypt cryptoSymmetricProvider = new TCrypt())
            {
                // Configure SymmetricProvider Key, IVKey, Mode and Padding
                ConfigureCryptoSymmetricProvider(cryptoKeyArray, cryptoIVKeyArray, cryptoSymmetricProvider);

                using (ICryptoTransform cTransform = cryptoSymmetricProvider.CreateDecryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                    //Release resources held by AesCng Encryptor                
                    cryptoSymmetricProvider.Clear();

                    // String of Json Decrypted
                    string jsonDecrypted = Encoding.UTF8.GetString(resultArray);

                    // Deserialize the Json en return the entity
                    return JsonConvert.DeserializeObject<T>(jsonDecrypted);
                }
            }
        }

        static void BuildKeysAndIV(out byte[] cryptoKeyArray, out byte[] cryptoIVKeyArray, bool useHashing = true)
        {
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                using (THasher hasherAlghorithm = new THasher())
                {
                    cryptoKeyArray = hasherAlghorithm.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
                    cryptoIVKeyArray = hasherAlghorithm.ComputeHash(Encoding.UTF8.GetBytes(CryptoIVKey));

                    //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                    hasherAlghorithm.Clear();
                }
            }
            else
            {
                cryptoKeyArray = Encoding.UTF8.GetBytes(CryptoKey);
                cryptoIVKeyArray = Encoding.UTF8.GetBytes(CryptoIVKey);
            }
        }

        static void ConfigureCryptoSymmetricProvider(byte[] cryptoKeyArray, byte[] cryptoIVKeyArray, TCrypt cryptoSimmetricProvider)
        {
            // Must resize IV because the lenght is 2x than supported 
            Array.Resize(ref cryptoKeyArray, cryptoSimmetricProvider.Key.Length);
            Array.Resize(ref cryptoIVKeyArray, cryptoSimmetricProvider.IV.Length);

            cryptoSimmetricProvider.Key = cryptoKeyArray;
            cryptoSimmetricProvider.IV = cryptoIVKeyArray;
            cryptoSimmetricProvider.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            cryptoSimmetricProvider.Padding = PaddingMode.PKCS7;
        }
    }

}
