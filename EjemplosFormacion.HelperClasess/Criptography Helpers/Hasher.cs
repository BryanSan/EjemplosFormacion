using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2013/10/28/hashing-algorithms-and-their-practical-usage-in-net-part-1/
    /// </summary>
    public class Hasher<THasher> : IHasher<THasher> where THasher : HashAlgorithm, new()
    {
        readonly THasher _hasherAlghorithm;

        public Hasher()
        {
            // Hasher a usar
            _hasherAlghorithm = new THasher();
        }

        public byte[] GetByteHash<T>(T objectToEncrypt)
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToEncrypt);

            // Obtenemos el byte del objeto serializado
            byte[] byteValue = Encoding.UTF8.GetBytes(serializedEntity);

            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(objectToEncrypt);

            return bytesHash;
        }

        public byte[] GetByteHash(byte[] byteValue)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            byte[] byteHash = _hasherAlghorithm.ComputeHash(byteValue);

            return byteHash;
        }

        public string GetHash<T>(T objectToEncrypt)
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(objectToEncrypt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash(byte[] byteValue)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(byteValue);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                    _hasherAlghorithm.Clear();
                    _hasherAlghorithm.Dispose();
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
