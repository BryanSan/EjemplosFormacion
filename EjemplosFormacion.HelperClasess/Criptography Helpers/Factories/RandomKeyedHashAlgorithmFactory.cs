using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Factories
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/14/using-hmacs-to-authenticate-a-hash-in-net/
    /// </summary>
    class RandomKeyedHashAlgorithmFactory<TKeyedHashAlgorithm> : IHashAlgorithmFactory<TKeyedHashAlgorithm>
        where TKeyedHashAlgorithm : KeyedHashAlgorithm, new()
    {
        readonly ICryptographicKeyGenerator _cryptographicKeyGenerator;
        readonly int _keyLength;

        public RandomKeyedHashAlgorithmFactory(ICryptographicKeyGenerator cryptographicKeyGenerator, int keyLength)
        {
            _cryptographicKeyGenerator = cryptographicKeyGenerator;
            _keyLength = keyLength;
        }

        public TKeyedHashAlgorithm CreateHashAlgorithm()
        {
            // Usamos la key pasada al constructor para generar mas aletoriedad con el hasher algorithm
            var keyedHashAlgorithm = new TKeyedHashAlgorithm();
            keyedHashAlgorithm.Key = _cryptographicKeyGenerator.GenerateCryptographicKeyBytes(_keyLength);

            return keyedHashAlgorithm;
        }
    }
}
