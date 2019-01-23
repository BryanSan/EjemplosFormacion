using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenValidatorWithAsymmetricKeyPair : IDisposable
    {
        bool ValidateToken(string token);
    }
}