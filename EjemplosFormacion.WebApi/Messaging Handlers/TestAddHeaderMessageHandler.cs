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

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Añadimos el header al Request
            request.Headers.Add("X-Custom-Header-Request", "This is my custom header in request.");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Añadimos el header al Response
            response.Headers.Add("X-Custom-Header-Response", "This is my custom header in response.");

            return response;
        }
    }
}