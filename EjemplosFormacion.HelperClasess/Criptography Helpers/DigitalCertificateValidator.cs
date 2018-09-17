using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System.Security.Cryptography.X509Certificates;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class DigitalCertificateValidator : IDigitalCertificateValidator
    {
        public DigitalCertificateValidatorResult Validate(X509Certificate2 certificateToValidate, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, X509VerificationFlags verificationFlags)
        {
            using (X509Chain chain = new X509Chain())
            {
                var chainPolicy = new X509ChainPolicy()
                {
                    RevocationMode = revocationMode,
                    RevocationFlag = revocationFlag,
                    VerificationFlags = verificationFlags
                };
                chain.ChainPolicy = chainPolicy;

                var validationResult = new DigitalCertificateValidatorResult();
                validationResult.Valid = chain.Build(certificateToValidate);
                if (validationResult.Valid == false)
                {
                    foreach (X509ChainElement chainElement in chain.ChainElements)
                    {
                        foreach (X509ChainStatus chainStatus in chainElement.ChainElementStatus)
                        {
                            validationResult.StatusInformation.Add(chainStatus.StatusInformation);
                        }
                    }
                }

                return validationResult;
            }
        }
    }
}