using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    public class AddChallengeOnUnauthorizedActionResult : IHttpActionResult
    {
        public AuthenticationHeaderValue Challenge { get; private set; }

        public IHttpActionResult InnerResult { get; private set; }

        public AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes schema, IHttpActionResult innerResult)
        {
            Challenge = new AuthenticationHeaderValue(schema.ToString());
            InnerResult = innerResult;
        }

        // Si el Response tiene el StatusCode como UnAuthorized entonces agregamos un Header con el Schema que se uso para intentar authenticar el Request
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }

    }
}