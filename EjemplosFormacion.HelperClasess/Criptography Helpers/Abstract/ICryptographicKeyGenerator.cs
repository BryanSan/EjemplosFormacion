using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ICryptographicKeyGenerator : IDisposable
    {
        string GenerateCryptographicKey(int keyLength);
        byte[] GenerateCryptographicKeyBytes(int keyLength);
    }
}