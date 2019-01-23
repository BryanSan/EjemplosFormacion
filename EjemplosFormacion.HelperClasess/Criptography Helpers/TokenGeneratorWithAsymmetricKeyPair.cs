using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class TokenGeneratorWithAsymmetricKeyPair : ITokenGeneratorWithAsymmetricKeyPair
    {
        readonly string _issuer;
        readonly string _audience;
        readonly int _expiresMinutes;
        readonly Lazy<RSA> _publicPrivateKeyPairCipher;
        readonly SigningCredentials _signingCredentials;

        public TokenGeneratorWithAsymmetricKeyPair(string publicPrivateKeyPair, string issuer, string audience, int expiresMinutes)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _publicPrivateKeyPairCipher = new Lazy<RSA>(() =>
            {
                var cipher = RSA.Create();
                cipher.FromXmlString(publicPrivateKeyPair);

                return cipher;
            });

            var securityKey = new RsaSecurityKey(_publicPrivateKeyPairCipher.Value);
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            _issuer = issuer;
            _audience = audience;
            _expiresMinutes = expiresMinutes;
        }

        public string GenerateTokenJwt(Dictionary<string, string> claims)
        {
            List<Claim> listOfClaims = claims.Select(x => new Claim(x.Key, x.Value)).ToList();

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(listOfClaims);

            // create token to the user
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            DateTime now = DateTime.UtcNow;
            JwtSecurityToken jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: _audience,
                issuer: _issuer,
                subject: claimsIdentity,
                notBefore: now,
                expires: now.AddMinutes(_expiresMinutes),
                signingCredentials: _signingCredentials);

            string jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_publicPrivateKeyPairCipher.IsValueCreated)
                    {
                        _publicPrivateKeyPairCipher.Value.Clear();
                        _publicPrivateKeyPairCipher.Value.Dispose();
                    }    
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