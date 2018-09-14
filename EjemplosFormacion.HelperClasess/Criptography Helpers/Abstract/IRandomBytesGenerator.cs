using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IRandomBytesGenerator : IDisposable
    {
        void GenerateRandomBytes(byte[] buffer);
    }
}
