using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using Microsoft.Owin.Security;
using System;
using System.Linq;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    public class TestCustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        readonly ITokenGeneratorWithSymmetricKey _tokenGeneratorWithSymmetricKey;

        public TestCustomJwtFormat(ITokenGeneratorWithSymmetricKey tokenGeneratorWithSymmetricKey)
        {
            _tokenGeneratorWithSymmetricKey = tokenGeneratorWithSymmetricKey;
        }

        public string Protect(AuthenticationTicket data)
        {
            string jwt = _tokenGeneratorWithSymmetricKey.GenerateTokenJwt(data.Identity.Claims.ToList());

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}