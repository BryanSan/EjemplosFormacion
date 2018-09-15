using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ISymmetricService<TSymmetricAlgorithm> : IDisposable
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        string Key { get; }
        string IV { get; }

        byte[] EncryptBytes(byte[] bytesToEncrypt);
        byte[] EncryptBytes<T>(T objectToEncrypt) where T : class;

        string Encrypt(byte[] bytesToEncrypt);
        string Encrypt<T>(T objectToEncrypt) where T : class;

        T Decrypt<T>(byte[] cipherBytes) where T : class;
        T Decrypt<T>(string cipherString) where T : class;
    }
}
