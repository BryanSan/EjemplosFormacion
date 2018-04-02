using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestCookiesMessageHandler : DelegatingHandler
    {
        const string SessionIdToken = "session-id";

        // Passing the next Handler of the Pipeline If Any
        public TestCookiesMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        public TestCookiesMessageHandler()
        {

        }

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Try to get the session ID from the request; otherwise create a new ID.
            CookieHeaderValue cookie = request.Headers.GetCookies(SessionIdToken).FirstOrDefault();
            CookieState cookieState = cookie?[SessionIdToken];
            string sessionId = cookieState?.Value;

            // If not session ID is found we create a new one
            if (string.IsNullOrWhiteSpace(sessionId) || !Guid.TryParse(sessionId, out _))
            {
                sessionId = Guid.NewGuid().ToString();
            }

            // Store the session ID in the request property bag.
            request.Properties[SessionIdToken] = sessionId;

            // Continue processing the HTTP request.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Set the session ID as a cookie in the response message.
            response.Headers.AddCookies(new CookieHeaderValue[] { new CookieHeaderValue(SessionIdToken, sessionId) });

            return response;
        }

    }
}