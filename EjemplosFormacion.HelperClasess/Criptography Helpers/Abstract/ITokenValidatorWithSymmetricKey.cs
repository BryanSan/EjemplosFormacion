using System.Security.Claims;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenValidatorWithSymmetricKey
    {
        (bool, ClaimsPrincipal) ValidateToken(string token);
    }
}