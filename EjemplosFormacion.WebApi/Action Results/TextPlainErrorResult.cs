using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Http Action Result usada para devolver un Error en Texto Puro
    /// </summary>
    class TextPlainErrorResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; private set; }

        public string Content { get; private set; }

        public TextPlainErrorResult(HttpRequestMessage request, string content)
        {
            Request = request;
            Content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            // Retornamos un simple Response con el Error y el Request usado
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            response.Content = new StringContent(Content);
            response.RequestMessage = Request;

            return Task.FromResult(response);
        }
    }
}