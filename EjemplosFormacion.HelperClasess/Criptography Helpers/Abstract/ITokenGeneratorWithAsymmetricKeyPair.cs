using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenGeneratorWithAsymmetricKeyPair : IDisposable
    {
        string GenerateTokenJwt(List<Claim> claims);
        string GenerateTokenJwt(Dictionary<string, string> claims);
    }
}