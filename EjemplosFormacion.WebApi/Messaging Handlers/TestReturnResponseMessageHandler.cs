using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler que devuelve un Response, interrumpiendo todo procesamiento del Pipeline del Web Api
    /// Como no llama al base.SendAsync este Message Handler no permite que pase la ejecucion a otro Inner Message Handler
    /// </summary>
    class TestReturnResponseMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestReturnResponseMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestReturnResponseMessageHandler()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Create the response.
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello!")
            };

            // Note: TaskCompletionSource creates a task that does not contain a delegate.
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);   // Also sets the task state to "RanToCompletion"

            // Another way
            var tsc2 = Task.FromResult(response);

            // Another way
            var tsc3 = Task<HttpResponseMessage>.Factory.StartNew(() => response);

            return tsc.Task;
        }

    }
}