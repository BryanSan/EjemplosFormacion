using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/18/hashing-passwords-with-a-password-based-key-derivation-function-in-net/
    /// </summary>
    public class HasherWithSaltAndDerivationKey : IHasherWithSaltAndDerivationKey
    {
        public byte[] GetByteHash(byte[] bytesToHash, byte[] salt, int roundOfHashIterations)
        {
            if (bytesToHash == null || bytesToHash.Count() <= 0) throw new ArgumentException($"{nameof(bytesToHash)} a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentException($"{nameof(salt)} a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            using (Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(bytesToHash, salt, roundOfHashIterations))
            {
                byte[] derivedBytes = pbkdf.GetBytes(32);

                return derivedBytes;
            }
        }

        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        public byte[] GetByteHash<T>(T objectToHash, string salt, int roundOfHashIterations)
        {
            if (objectToHash == null) throw new ArgumentException($"{nameof(objectToHash)} a hashear no puede estar vacio!.");
            if (string.IsNullOrWhiteSpace(salt)) throw new ArgumentException($"{nameof(salt)} a hashear no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToHash);

            // Obtenemos el byte del objeto serializado
            byte[] serializedEntityBytes = Encoding.UTF8.GetBytes(serializedEntity);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Obtenemos el Hash
            byte[] derivedBytes = GetByteHash(serializedEntityBytes, saltBytes, roundOfHashIterations);

            return derivedBytes;
        }

        public string GetHash(byte[] bytesToHash, byte[] salt, int roundOfHashIterations)
        {
            byte[] derivedBytes = GetByteHash(bytesToHash, salt, roundOfHashIterations);
            string derivedBytesString = Convert.ToBase64String(derivedBytes);

            return derivedBytesString;
        }

        public string GetHash<T>(T objectToHash, string salt, int roundOfHashIterations)
        {
            byte[] derivedBytes = GetByteHash(objectToHash, salt, roundOfHashIterations);
            string derivedBytesString = Convert.ToBase64String(derivedBytes);

            return derivedBytesString;
        }
    }
}