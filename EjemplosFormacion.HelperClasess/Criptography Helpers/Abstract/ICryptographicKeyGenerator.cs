using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ICryptographicKeyGenerator : IDisposable
    {
        byte[] GenerateCryptographicKeyBytes(int keyLength);
        string GenerateCryptographicKey(int keyLength);
    }
}