namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface ITokenValidatorWithSymmetricKey
    {
        bool ValidateToken(string token);
    }
}