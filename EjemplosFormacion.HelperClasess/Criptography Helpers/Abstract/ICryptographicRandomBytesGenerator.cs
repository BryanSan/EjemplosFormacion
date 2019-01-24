using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ICryptographicRandomBytesGenerator : IDisposable
    {
        void GenerateRandomBytes(byte[] bufferToFill);
    }
}
