using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    public class AuthenticationFailureActionResult : IHttpActionResult
    {

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public AuthenticationFailureActionResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        // Personalizamos el Response con los mensajes y el Request con el que dio UnAuthorized
        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }
}