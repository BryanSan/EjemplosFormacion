using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para leer los valores del Query String 
    /// </summary>
    class TestReadQueryStringMessagingHandler : DelegatingHandler
    {
        readonly string _key;

        // Passing the next Handler of the Pipeline If Any
        public TestReadQueryStringMessagingHandler(HttpMessageHandler messageHandler, string key) : base(messageHandler)
        {
            _key = key;
        }

        public TestReadQueryStringMessagingHandler(string key)
        {
            _key = key;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Valido el parametro Key en el Query String del Request
            if (!ValidateKey(request))
            {
                // Si no existe el Query String buscado se devuelve un response con el Status Code de Forbidden (Se niega a procesar la solicitud)
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        bool ValidateKey(HttpRequestMessage message)
        {
            // Busca en el Query String el parametro key y lo valido
            var query = message.RequestUri.ParseQueryString();
            string key = query["key"];
            return (key == _key);
        }

    }
}