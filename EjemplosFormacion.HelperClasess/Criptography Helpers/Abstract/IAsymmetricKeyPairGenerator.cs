using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IAsymmetricKeyPairGenerator<TAsymmetricAlgorithm>
        where TAsymmetricAlgorithm : AsymmetricAlgorithm, new()
    {
        AsymmetricKeyPairGenerationResult GenerateKeysAsXml(int keySizeBits);
    }
}