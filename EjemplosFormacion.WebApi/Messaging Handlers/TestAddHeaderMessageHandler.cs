using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestAddHeaderMessageHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Custom-Header-Request", "This is my custom header in request.");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            response.Headers.Add("X-Custom-Header-Response", "This is my custom header in response.");

            return response;
        }
    }
}