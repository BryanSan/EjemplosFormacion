using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2013/10/28/hashing-algorithms-and-their-practical-usage-in-net-part-1/
    /// https://dotnetcodr.com/2013/10/31/hashing-algorithms-and-their-practical-usage-in-net-part-2/
    /// https://dotnetcodr.com/2016/10/17/how-to-hash-passwords-with-a-salt-in-net/
    /// </summary>
    public class Hasher<THashAlgorithm> : IHasher<THashAlgorithm> 
        where THashAlgorithm : HashAlgorithm, new()
    {
        readonly Lazy<THashAlgorithm> _hashAlghorithm;

        public string HashAlgorithmName => "SHA1";

        public Hasher(IHashAlgorithmFactory<THashAlgorithm> hashAlgorithmFactory)
        {
            // Usamos las Factories de ayuda para obtener una instancia del hasher
            _hashAlghorithm = new Lazy<THashAlgorithm>(() => hashAlgorithmFactory.CreateHashAlgorithm());
        }

        #region GetByteHash
        public byte[] GetByteHash(byte[] bytesToHash)
        {
            if (bytesToHash == null || bytesToHash.Count() <= 0) throw new ArgumentException($"{nameof(bytesToHash)} a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            byte[] byteHashed = _hashAlghorithm.Value.ComputeHash(bytesToHash);

            return byteHashed;
        }

        public byte[] GetByteHash(byte[] bytesToHash, byte[] salt)
        {
            if (bytesToHash == null || bytesToHash.Count() <= 0) throw new ArgumentException($"{nameof(bytesToHash)} a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentException($"{nameof(salt)} a hashear no puede estar vacio!.");

            var bytesListToHash = new List<byte>(bytesToHash);
            bytesListToHash.AddRange(salt);

            // Obtenemos el Hash
            byte[] byteHashed = _hashAlghorithm.Value.ComputeHash(bytesListToHash.ToArray());

            return byteHashed;
        }

        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        // El entropy segun las buenas practicas deberia ser un valor constante reusable (sea random generado o algo elegido por el developer)
        public byte[] GetByteHash(byte[] bytesToHash, byte[] salt, byte[] entropy)
        {
            if (bytesToHash == null || bytesToHash.Count() <= 0) throw new ArgumentException($"{nameof(bytesToHash)} a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentException($"{nameof(salt)} a hashear no puede estar vacio!.");
            if (entropy == null || entropy.Count() <= 0) throw new ArgumentException($"{nameof(entropy)} a hashear no puede estar vacio!.");

            var bytesListToHash = new List<byte>(bytesToHash);
            bytesListToHash.AddRange(salt);
            bytesListToHash.AddRange(entropy);

            // Obtenemos el Hash
            byte[] byteHashed = _hashAlghorithm.Value.ComputeHash(bytesListToHash.ToArray());

            return byteHashed;
        }
        public byte[] GetByteHash<T>(T objectToHash)
        {
            byte[] bytesHashed = GetByteHash(objectToHash, null, null);
            return bytesHashed;
        }

        public byte[] GetByteHash<T>(T objectToHash, string salt)
        {
            byte[] bytesHashed = GetByteHash(objectToHash, salt, null);
            return bytesHashed;
        }

        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        // El entropy segun las buenas practicas deberia ser un valor constante reusable (sea random generado o algo elegido por el developer)
        public byte[] GetByteHash<T>(T objectToHash, string salt, string entropy)
        {
            if (objectToHash == null) throw new ArgumentException($"{nameof(objectToHash)} a hashear no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToHash);

            // Obtenemos el byte del objeto serializado + el salt + el entropy
            byte[] bytesToHash = Encoding.UTF8.GetBytes(serializedEntity + salt + entropy);

            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(bytesToHash);

            return bytesHashed;
        }
        #endregion

        #region GetHash
        public string GetHash(byte[] bytesToHash)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(bytesToHash);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }

        public string GetHash(byte[] bytesToHash, byte[] salt)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(bytesToHash, salt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }

        public string GetHash(byte[] bytesToHash, byte[] salt, byte[] entropy)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(bytesToHash, salt, entropy);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }

        public string GetHash<T>(T objectToHash)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(objectToHash);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }

        public string GetHash<T>(T objectToHash, string salt)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(objectToHash, salt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }

        public string GetHash<T>(T objectToHash, string salt, string entropy)
        {
            // Obtenemos el Hash
            byte[] bytesHashed = GetByteHash(objectToHash, salt, entropy);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHashed);

            return base64hash;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_hashAlghorithm.IsValueCreated)
                    {
                        //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                        _hashAlghorithm.Value.Clear();
                        _hashAlghorithm.Value.Dispose();
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
