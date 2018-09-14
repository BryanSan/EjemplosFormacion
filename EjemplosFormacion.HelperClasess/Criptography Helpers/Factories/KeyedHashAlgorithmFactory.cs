using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Factories
{
    class KeyedHashAlgorithmFactory<TKeyedHashAlgorithm> : IHashAlgorithmFactory<TKeyedHashAlgorithm>
        where TKeyedHashAlgorithm : KeyedHashAlgorithm, new()
    {
        readonly byte[] _keyArray;

        public KeyedHashAlgorithmFactory(string key)
        {
            _keyArray = Encoding.UTF8.GetBytes(key);
        }

        public TKeyedHashAlgorithm CreateHashAlgorithm()
        {
            // Usamos la key pasada al constructor para generar mas aletoriedad con el hasher algorithm
            var keyedHashAlgorithm = new TKeyedHashAlgorithm();
            keyedHashAlgorithm.Key = _keyArray;

            return keyedHashAlgorithm;
        }
    }
}
