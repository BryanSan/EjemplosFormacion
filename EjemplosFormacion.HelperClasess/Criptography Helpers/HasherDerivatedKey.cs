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
    public class HasherDerivatedKey : IHasherDerivatedKey
    {
        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        public byte[] GetByteHash<T>(T objectToEncrypt, string salt, int roundOfHashIterations)
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");
            if (string.IsNullOrWhiteSpace(salt)) throw new ArgumentNullException("Salt a hashear no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToEncrypt);

            // Obtenemos el byte del objeto serializado
            byte[] serializedEntityBytes = Encoding.UTF8.GetBytes(serializedEntity);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Obtenemos el Hash
            byte[] derivedBytes = GetByteHash(serializedEntityBytes, saltBytes, roundOfHashIterations);

            return derivedBytes;
        }

        public byte[] GetByteHash(byte[] byteValue, byte[] salt, int roundOfHashIterations)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentNullException("Salt a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            using (Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(byteValue, salt, roundOfHashIterations))
            {
                byte[] derivedBytes = pbkdf.GetBytes(32);

                return derivedBytes;
            }
        }

        public string GetHash<T>(T objectToEncrypt, string salt, int roundOfHashIterations)
        {
            byte[] derivedBytes = GetByteHash(objectToEncrypt, salt, roundOfHashIterations);
            string derivedBytesString = Convert.ToBase64String(derivedBytes);

            return derivedBytesString;
        }

        public string GetHash(byte[] byteValue, byte[] salt, int roundOfHashIterations)
        {
            byte[] derivedBytes = GetByteHash(byteValue, salt, roundOfHashIterations);
            string derivedBytesString = Convert.ToBase64String(derivedBytes);

            return derivedBytesString;
        }
    }
}