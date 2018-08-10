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

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
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