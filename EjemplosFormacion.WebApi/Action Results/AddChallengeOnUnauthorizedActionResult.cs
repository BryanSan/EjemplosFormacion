using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Action Result para añadir el header de Challenge (WwwAuthenticate) cuando una respuesta no es Autorizada
    /// Para que el cliente sepa con que Schema el Servidor rechazo su Request
    /// </summary>
    class AddChallengeOnUnauthorizedActionResult : IHttpActionResult
    {
        readonly AuthenticationHeaderValue _challenge;
        readonly IHttpActionResult _innerResult;
        readonly HttpResponseMessage _innerResponse;

        private AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes schema)
        {
            _challenge = new AuthenticationHeaderValue(schema.ToString());
        }

        public AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes schema, IHttpActionResult innerResult) : this(schema)
        {
            _innerResult = innerResult ?? throw new ArgumentException("innerResult vacio!.");
        }

        public AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes schema, HttpResponseMessage innerResponse) : this(schema)
        {
            _innerResponse = innerResponse ?? throw new ArgumentException("innerResponse vacio!.");
        }

        // Si el Response tiene el StatusCode como UnAuthorized entonces agregamos un Header con el Schema que se uso para intentar authenticar el Request
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            // Si ya tengo el response entonces la uso
            HttpResponseMessage response = _innerResponse;

            // Si no tengo response entonces busco en ejecuto mi ActionResult para obtener el response
            if (_innerResult != null)
            {
                response = await _innerResult.ExecuteAsync(cancellationToken);
            } 

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                // El WwwAuthenticate sirve para indicarle al Client contra que Authentication Schema se esta haciendo la Authentication
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == _challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(_challenge);
                }
            }

            return response;
        }

    }
}