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
    public class TestReturnResponseMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestReturnResponseMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        public TestReturnResponseMessageHandler()
        {

        }

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
            return tsc.Task;
        }

    }
}