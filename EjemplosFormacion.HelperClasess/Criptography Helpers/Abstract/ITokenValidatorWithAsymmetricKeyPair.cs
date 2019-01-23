using System;
using System.Security.Claims;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenValidatorWithAsymmetricKeyPair : IDisposable
    {
        (bool, ClaimsPrincipal) ValidateToken(string token);
    }
}