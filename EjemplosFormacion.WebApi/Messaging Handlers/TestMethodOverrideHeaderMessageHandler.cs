using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para leer el Header X-HTTP-Method-Override y actualiza el metodo (HttpVerb) del Request con el valor del Header X-HTTP-Method-Override
    /// </summary>
    class TestMethodOverrideHeaderMessageHandler : DelegatingHandler
    {
        readonly string[] _methods = { "DELETE", "HEAD", "PUT" };
        const string _header = "X-HTTP-Method-Override";

        // Passing the next Handler of the Pipeline If Any
        public TestMethodOverrideHeaderMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestMethodOverrideHeaderMessageHandler()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check for HTTP POST with the X-HTTP-Method-Override header.
            if (request.Method == HttpMethod.Post && request.Headers.Contains(_header))
            {
                // Check if the header value is in our methods list.
                var method = request.Headers.GetValues(_header).FirstOrDefault();
                if (_methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method.
                    request.Method = new HttpMethod(method);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

    }
}