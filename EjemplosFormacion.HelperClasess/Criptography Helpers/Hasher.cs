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
        readonly THashAlgorithm _hasherAlghorithm;

        public Hasher(IHashAlgorithmFactory<THashAlgorithm> hashAlgorithmFactory)
        {
            // Usamos las Factories de ayuda para obtener una instancia del hasher
            _hasherAlghorithm = hashAlgorithmFactory.CreateHashAlgorithm();
        }

        #region GetByteHash
        public byte[] GetByteHash<T>(T objectToEncrypt)
        {
            return GetByteHash(objectToEncrypt, null, null);
        }

        public byte[] GetByteHash<T>(T objectToEncrypt, string salt)
        {
            return GetByteHash(objectToEncrypt, salt, null);
        }

        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        // El entropy segun las buenas practicas deberia ser un valor constante reusable (sea random generado o algo elegido por el developer)
        public byte[] GetByteHash<T>(T objectToEncrypt, string salt, string entropy)
        {
            if (objectToEncrypt == null) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToEncrypt);

            // Obtenemos el byte del objeto serializado + el salt + el entropy
            byte[] byteValue = Encoding.UTF8.GetBytes(serializedEntity + salt + entropy);

            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(byteValue);

            return bytesHash;
        }

        public byte[] GetByteHash(byte[] byteValue)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");

            // Obtenemos el Hash
            byte[] byteHash = _hasherAlghorithm.ComputeHash(byteValue);

            return byteHash;
        }

        public byte[] GetByteHash(byte[] byteValue, byte[] salt)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentNullException("Salt a hashear no puede estar vacio!.");

            List<byte> bytesToHash = new List<byte>(byteValue);
            bytesToHash.AddRange(salt);

            // Obtenemos el Hash
            byte[] byteHash = _hasherAlghorithm.ComputeHash(bytesToHash.ToArray());

            return byteHash;
        }

        // Salt segun las buenas practicas deberia ser un valor random generado individualmente (no reusable)
        // El entropy segun las buenas practicas deberia ser un valor constante reusable (sea random generado o algo elegido por el developer)
        public byte[] GetByteHash(byte[] byteValue, byte[] salt, byte[] entropy)
        {
            if (byteValue == null || byteValue.Count() <= 0) throw new ArgumentNullException("Mensaje a hashear no puede estar vacio!.");
            if (salt == null || salt.Count() <= 0) throw new ArgumentNullException("Salt a hashear no puede estar vacio!.");
            if (entropy == null || entropy.Count() <= 0) throw new ArgumentNullException("Entropy a hashear no puede estar vacio!.");

            List<byte> bytesToHash = new List<byte>(byteValue);
            bytesToHash.AddRange(salt);
            bytesToHash.AddRange(entropy);
            
            // Obtenemos el Hash
            byte[] byteHash = _hasherAlghorithm.ComputeHash(bytesToHash.ToArray());

            return byteHash;
        }
        #endregion

        #region GetHash
        public string GetHash<T>(T objectToEncrypt)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(objectToEncrypt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash<T>(T objectToEncrypt, string salt)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(objectToEncrypt, salt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash<T>(T objectToEncrypt, string salt, string entropy)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(objectToEncrypt, salt, entropy);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash(byte[] byteValue)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(byteValue);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash(byte[] byteValue, byte[] salt)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(byteValue, salt);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

            return base64hash;
        }

        public string GetHash(byte[] byteValue, byte[] salt, byte[] entropy)
        {
            // Obtenemos el Hash
            byte[] bytesHash = GetByteHash(byteValue, salt, entropy);

            // Obtenemos el base64 del hash
            string base64hash = Convert.ToBase64String(bytesHash);

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
