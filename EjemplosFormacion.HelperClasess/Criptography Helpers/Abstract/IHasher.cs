using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHasher<THashAlgorithm> : IDisposable
        where THashAlgorithm : HashAlgorithm, new()
    {
        string HashAlgorithmName  { get; }

        byte[] GetByteHash(byte[] bytesToHash);
        byte[] GetByteHash(byte[] bytesToHash, byte[] salt);
        byte[] GetByteHash(byte[] bytesToHash, byte[] salt, byte[] entropy);

        byte[] GetByteHash<T>(T objectToHash);
        byte[] GetByteHash<T>(T objectToHash, string salt);
        byte[] GetByteHash<T>(T objectToHash, string salt, string entropy);

        string GetHash(byte[] bytesToHash);
        string GetHash(byte[] bytesToHash, byte[] salt);
        string GetHash(byte[] bytesToHash, byte[] salt, byte[] entropy);

        string GetHash<T>(T objectToHash);
        string GetHash<T>(T objectToHash, string salt);
        string GetHash<T>(T objectToHash, string salt, string entropy);
    }
}