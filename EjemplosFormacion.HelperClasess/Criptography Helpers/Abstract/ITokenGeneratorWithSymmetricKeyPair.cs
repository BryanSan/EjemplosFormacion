using System;
using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    interface ITokenGeneratorWithSymmetricKeyPair
    {
        string GenerateTokenJwt(Dictionary<string, string> claims);
    }
}
