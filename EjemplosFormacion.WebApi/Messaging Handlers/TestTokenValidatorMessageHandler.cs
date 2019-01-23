using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestTokenValidatorMessageHandler : DelegatingHandler
    {
        readonly ITokenValidatorWithSymmetricKey _tokenValidator;

        public TestTokenValidatorMessageHandler(ITokenValidatorWithSymmetricKey tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token;

            // determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                return base.SendAsync(request, cancellationToken);
            }

            (bool isValid, ClaimsPrincipal claimsPrincipal) = _tokenValidator.ValidateToken(token);

            // Extract and assign Current Principal and user
            if (isValid)
            {
                Thread.CurrentPrincipal = claimsPrincipal;
                HttpContext.Current.User = claimsPrincipal;

                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}