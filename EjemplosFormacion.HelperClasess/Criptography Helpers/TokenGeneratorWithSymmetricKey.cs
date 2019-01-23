using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class TokenGeneratorWithSymmetricKey : ITokenGeneratorWithSymmetricKey
    {
        readonly string _issuer;
        readonly string _audience;
        readonly int _expiresMinutes;
        readonly SigningCredentials _signingCredentials;

        public TokenGeneratorWithSymmetricKey(string secretKey, string issuer, string audience, int expiresMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
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
    }
}