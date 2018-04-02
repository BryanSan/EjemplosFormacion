using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    public class TestReadHeaderMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestReadHeaderMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        public TestReadHeaderMessageHandler()
        {

        }

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // El Request tiene varias propiedades para leer Headers que son comunes, como Accept, Authorization, etc...
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypeHeaderList = request.Headers.Accept;
            MediaTypeWithQualityHeaderValue mediaTypeHeader = mediaTypeHeaderList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptChartSetList = request.Headers.AcceptCharset;
            StringWithQualityHeaderValue acceptChartSet = acceptChartSetList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptEncodingList = request.Headers.AcceptEncoding;
            StringWithQualityHeaderValue acceptEncoding = acceptEncodingList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptLanguageList = request.Headers.AcceptLanguage;
            StringWithQualityHeaderValue acceptLanguage = acceptLanguageList.FirstOrDefault();

            AuthenticationHeaderValue authenticationHeaderValue = request.Headers.Authorization;

            // Si quieres buscar un Custom header solo buscalo por su Text Key, cuidado con una Excepcion por si el Header no existe
            IEnumerable<string> headerValuesRequest;
            var customHeaderTextRequest = string.Empty;

            if (request.Headers.TryGetValues("customHeaderRequest", out headerValuesRequest))
            {
                customHeaderTextRequest = headerValuesRequest.FirstOrDefault();
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // El Response tiene menos Headers comunes
            HttpHeaderValueCollection<string> acceptRangeHeaderList = response.Headers.AcceptRanges;
            HttpHeaderValueCollection<ProductInfoHeaderValue> serverHeaderList = response.Headers.Server;

            // Si quieres buscar un Custom header solo buscalo por su Text Key, cuidado con una Excepcion por si el Header no existe
            IEnumerable<string> headerValuesResponse;
            var customHeaderTextResponse = string.Empty;

            if (response.Headers.TryGetValues("customHeaderResponse", out headerValuesResponse))
            {
                customHeaderTextResponse = headerValuesResponse.FirstOrDefault();
            }

            return response;
        }
    }
}