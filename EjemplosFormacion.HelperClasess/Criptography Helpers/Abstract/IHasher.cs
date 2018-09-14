using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHasher<THashAlgorithm> : IDisposable
        where THashAlgorithm : HashAlgorithm, new()
    {
        byte[] GetByteHash<T>(T value);
        byte[] GetByteHash<T>(T value, string salt);
        byte[] GetByteHash<T>(T value, string salt, string entropy);

        byte[] GetByteHash(byte[] value);
        byte[] GetByteHash(byte[] value, byte[] salt);
        byte[] GetByteHash(byte[] value, byte[] salt, byte[] entropy);

        string GetHash<T>(T value);
        string GetHash<T>(T value, string salt);
        string GetHash<T>(T value, string salt, string entropy);

        string GetHash(byte[] value);
        string GetHash(byte[] value, byte[] salt);
        string GetHash(byte[] value, byte[] salt, byte[] entropy);
    }
}