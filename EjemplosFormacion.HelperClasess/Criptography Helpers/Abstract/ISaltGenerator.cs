using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ISaltGenerator : IDisposable
    {
        byte[] GenerateSaltBytes(int saltLength);

        string GenerateSalt(int saltLength);
    }
}
