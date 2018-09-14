using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IDigitalSignatureVerifier<THashAlgorithm> : IDisposable
        where THashAlgorithm : HashAlgorithm, new()
    {
        T ExtractMessage<T>(DigitalSignatureResult signatureResult);
        bool VerifySignature(DigitalSignatureResult signatureResult);
    }
}
