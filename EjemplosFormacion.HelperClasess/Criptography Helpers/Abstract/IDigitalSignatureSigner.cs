using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IDigitalSignatureSigner<THashAlgorithm> : IDisposable
        where THashAlgorithm : HashAlgorithm, new()
    {
        DigitalSignatureResult BuildSignedMessage<T>(T objectToSign);
        DigitalSignatureResult BuildSignedMessage(byte[] messageBytes);
    }
}
