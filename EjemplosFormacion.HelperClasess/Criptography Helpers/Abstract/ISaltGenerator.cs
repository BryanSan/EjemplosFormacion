using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ISaltGenerator : IDisposable
    {
        string GenerateSalt(int saltLength);

        byte[] GenerateSaltBytes(int saltLength);
    }
}
