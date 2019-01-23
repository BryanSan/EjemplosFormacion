using System;
using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    interface ITokenGeneratorWithSymmetricKey
    {
        string GenerateTokenJwt(Dictionary<string, string> claims);
    }
}
