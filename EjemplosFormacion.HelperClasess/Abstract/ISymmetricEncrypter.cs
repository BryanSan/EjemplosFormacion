using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.Abstract
{
    public interface ISymmetricEncrypter<TCrypt, THasher>
        where TCrypt : SymmetricAlgorithm, new()
        where THasher : HashAlgorithm, new()
    {
        string Encrypt<T>(T objectToEncrypt, bool useHashing = true);

        T Decrypt<T>(string cipherString, bool useHashing = true);
    }
}
