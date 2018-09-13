using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHashAlgorithmFactory<THashAlgorithm>
        where THashAlgorithm : HashAlgorithm, new()
    {
        THashAlgorithm CreateHashAlgorithm();
    }
}
