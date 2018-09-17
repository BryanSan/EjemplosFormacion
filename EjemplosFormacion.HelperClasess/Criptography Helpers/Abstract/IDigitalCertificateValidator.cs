using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System.Security.Cryptography.X509Certificates;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IDigitalCertificateValidator
    {
        DigitalCertificateValidatorResult Validate(X509Certificate2 certificateToValidate, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, X509VerificationFlags verificationFlags);
    }
}
