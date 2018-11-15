using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class TokenGenerator
    {
        readonly Lazy<RSA> _publicPrivateKeyPairCipher;
        readonly string _issuer;
        readonly string _audience;
        readonly DateTime _lifeTime;

        public TokenGenerator(string publicPrivateKeyPair, string issuer, string audience, DateTime lifeTime)
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

            _issuer = issuer;
            _audience = audience;
            _lifeTime = lifeTime;
        }

        //public string GenerateToken(Dictionary<string, string> claims)
        //{
        //    List<Claim> listOfClaims = claims.Select(x => new Claim(x.Key, x.Value)).ToList();

        //    JwtSecurityToken jwtToken = new JwtSecurityToken(_issuer, _audience, listOfClaims
        //        , lifetime: new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
        //        , signingCredentials: new SigningCredentials(new RsaSecurityKey(publicAndPrivate)
        //            , SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    string tokenString = tokenHandler.WriteToken(jwtToken);
        //}

    }
}
