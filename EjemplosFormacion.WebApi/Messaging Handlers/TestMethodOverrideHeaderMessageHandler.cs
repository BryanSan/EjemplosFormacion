using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestMethodOverrideHeaderMessageHandler : DelegatingHandler
    {
        readonly string[] _methods = { "DELETE", "HEAD", "PUT" };
        const string _header = "X-HTTP-Method-Override";

        // Passing the next Handler of the Pipeline If Any
        public TestMethodOverrideHeaderMessageHandler(DelegatingHandler delegatingHandler) : base(delegatingHandler)
        {

        }

        public TestMethodOverrideHeaderMessageHandler()
        {

        }

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