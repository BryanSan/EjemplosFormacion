using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para agregar un custom Header al Request y Response
    /// </summary>
    class TestAddHeaderMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestAddHeaderMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestAddHeaderMessageHandler()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Before Execute Action

            // Añadimos el header al Request
            request.Headers.Add("X-Custom-Header-Request", "This is my custom header in request.");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // After Execute Action

            // Añadimos el header al Response
            response.Headers.Add("X-Custom-Header-Response", "This is my custom header in response.");

            return response;
        }
    }
}