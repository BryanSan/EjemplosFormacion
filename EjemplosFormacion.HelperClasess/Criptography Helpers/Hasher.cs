using EjemplosFormacion.HelperClasess.Abstract;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class Hasher<THasher> : IHasher<THasher> where THasher : HashAlgorithm, new()
    {
        public string GetHash<T>(T value) where T : class
        {
            string serializedEntity = JsonConvert.SerializeObject(value);
            return GetHash(serializedEntity);
        }

        public string GetHash(string value)
        {
            using (THasher hasherAlghorithm = new THasher())
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(value);

                byte[] byteHash = hasherAlghorithm.ComputeHash(byteValue);

                string hash = Convert.ToBase64String(byteHash);

                //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                hasherAlghorithm.Clear();

                return hash;
            }
        }
    }
}
