using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ISymmetricEncrypter<TSymmetricAlgorithm> : IDisposable
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        string Encrypt<T>(T objectToEncrypt) where T : class;

        T Decrypt<T>(string cipherString) where T : class;
    }
}
