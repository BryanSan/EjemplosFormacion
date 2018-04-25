using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para ejecutar codigo en el Pipeline del Web Api cuando llega un Request
    /// Puedes hacer el CRUD al Request y Response
    /// </summary>
    class TestMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestMessageHandler()
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Before Execute Action

            // Pasa el procesamiento al siguiente handler, si no hay mas message handler va al Action destino
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            // After Execute Action

            // Si no necesitas hacer nada After Execute Action simplemente haz return base.SendAsync
            //return base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}