using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class TokenValidatorWithSymmetricKey : ITokenValidatorWithSymmetricKey
    {
        readonly string _issuer;
        readonly string _audience;
        readonly SymmetricSecurityKey _securityKey;
        readonly Func<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, bool> _lifeTimeValidator;

        public TokenValidatorWithSymmetricKey(string secretKey, string issuer, string audience, Func<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, bool> lifeTimeValidator)
        {
            _securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            _issuer = issuer;
            _audience = audience;
            _lifeTimeValidator = lifeTimeValidator;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = _audience,
                ValidIssuer = _issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters tokenValidationParameters) =>
                {
                    return _lifeTimeValidator(notBefore, expires, securityToken, tokenValidationParameters);
                }
            };

            try
            {
                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}
