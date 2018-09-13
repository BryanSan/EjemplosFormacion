using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Factories
{
    public class HashAlgorithmFactory<THashAlgorithm> : IHashAlgorithmFactory<THashAlgorithm>
        where THashAlgorithm : HashAlgorithm, new()
    {
        public THashAlgorithm CreateHashAlgorithm()
        {
            return new THashAlgorithm();
        }
    }
}