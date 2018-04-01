using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestReadQueryStringMessagingHandler : DelegatingHandler
    {
        readonly string _key;

        // Passing the next Handler of the Pipeline If Any
        public TestReadQueryStringMessagingHandler(DelegatingHandler delegatingHandler, string key = null) : base(delegatingHandler) 
        {
            _key = key;
        }

        public TestReadQueryStringMessagingHandler(string key) : this(null, key)
        {
            
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!ValidateKey(request))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateKey(HttpRequestMessage message)
        {
            var query = message.RequestUri.ParseQueryString();
            string key = query["key"];
            return (key == _key);
        }

    }
}