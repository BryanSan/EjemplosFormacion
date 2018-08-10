using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler que lee y añade Cookies 
    /// Usando el header Set-Cookie para añadir una Cookie
    /// Inspeccionar el Request para ver las Cookie disponibles
    /// Usa de ejemplo añadir el SessionId si no existe y leerlo si ya existe
    /// </summary>
    class TestCookiesMessageHandler : DelegatingHandler
    {
        const string SessionIdToken = "session-id";

        // Passing the next Handler of the Pipeline If Any
        public TestCookiesMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestCookiesMessageHandler()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Try to get the session ID from the request; otherwise create a new ID.
            CookieHeaderValue cookie = request.Headers.GetCookies(SessionIdToken).FirstOrDefault();
            CookieState cookieState = cookie?[SessionIdToken];
            string sessionId = cookieState?.Value;

            // If not session ID is found we create a new one
            if (string.IsNullOrWhiteSpace(sessionId) || !Guid.TryParse(sessionId, out _))
            {
                sessionId = Guid.NewGuid().ToString();
            }

            // Store the session ID in the request property bag.
            request.Properties[SessionIdToken] = sessionId;

            // Continue processing the HTTP request.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Set the session ID as a cookie in the response message.
            response.Headers.AddCookies(new CookieHeaderValue[] { new CookieHeaderValue(SessionIdToken, sessionId) });

            return response;
        }

    }
}