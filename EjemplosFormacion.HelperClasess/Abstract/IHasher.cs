using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.Abstract
{
    public interface IHasher<THasher> where THasher : HashAlgorithm, new()
    {
        string GetHash<T>(T value) where T : class;

        string GetHash(string value);
    }
}