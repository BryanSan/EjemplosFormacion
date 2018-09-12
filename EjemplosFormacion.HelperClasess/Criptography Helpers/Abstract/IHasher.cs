using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHasher<THasher> : IDisposable
        where THasher : HashAlgorithm, new()
    {
        byte[] GetByteHash<T>(T value);
        byte[] GetByteHash(byte[] value);

        string GetHash<T>(T value);
        string GetHash(byte[] value);
    }
}