using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class TokenValidatorWithAsymmetricKeyPair : ITokenValidatorWithAsymmetricKeyPair
    {
        readonly string _issuer;
        readonly string _audience;
        readonly RSA _publicPrivateKeyPairCipher;
        readonly RsaSecurityKey _securityKey;
        readonly Func<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, bool> _lifeTimeValidator;

        public TokenValidatorWithAsymmetricKeyPair(string publicPrivateKeyPair, string issuer, string audience, Func<DateTime?, DateTime?, SecurityToken, TokenValidationParameters, bool> lifeTimeValidator)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _publicPrivateKeyPairCipher = RSA.Create();
            _publicPrivateKeyPairCipher.FromXmlString(publicPrivateKeyPair);

            _securityKey = new RsaSecurityKey(_publicPrivateKeyPairCipher);
            _issuer = issuer;
            _audience = audience;
            _lifeTimeValidator = lifeTimeValidator;
        }

        public (bool, ClaimsPrincipal) ValidateToken(string token)
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
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return (true, claimsPrincipal);
            }
            catch (SecurityTokenException)
            {
                return (false, null);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _publicPrivateKeyPairCipher.Clear();
                    _publicPrivateKeyPairCipher.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
