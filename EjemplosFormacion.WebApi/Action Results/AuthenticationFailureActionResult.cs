using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Action Result para devolver un Response con el HttpStatusCode de Unauthorized junto con el Request que se envio
    /// </summary>
    public class AuthenticationFailureActionResult : IHttpActionResult
    {
        readonly string _reasonPhrase;
        readonly HttpRequestMessage _request;

        public AuthenticationFailureActionResult(string reasonPhrase, HttpRequestMessage request)
        {
            _reasonPhrase = reasonPhrase;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        // Personalizamos el Response con los mensajes y el Request con el que dio UnAuthorized
        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = _request;
            response.ReasonPhrase = _reasonPhrase;
            return response;
        }
    }
}