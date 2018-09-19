using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IAsymmetricKeyPairGenerator<TAsymmetricAlgorithm>
        where TAsymmetricAlgorithm : AsymmetricAlgorithm, new()
    {
        AsymmetricKeyPairGenerationResult GenerateKeysAsXml(int keySizeBits);

        AsymmetricKeyPairGenerationResult GenerateKeysFromCertificateAsXml(X509Certificate2 certificate)
    }
}