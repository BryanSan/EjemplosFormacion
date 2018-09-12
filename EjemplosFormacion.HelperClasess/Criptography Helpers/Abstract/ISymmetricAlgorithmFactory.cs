using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ISymmetricAlgorithmFactory<TSymmetricAlgorithm>
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        TSymmetricAlgorithm CreateSymmetricAlgorithm();
    }
}