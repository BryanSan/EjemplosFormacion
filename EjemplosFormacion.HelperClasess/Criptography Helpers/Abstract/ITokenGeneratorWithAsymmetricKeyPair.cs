using System;
using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenGeneratorWithAsymmetricKeyPair : IDisposable
    {
        string GenerateTokenJwt(Dictionary<string, string> claims);
    }
}