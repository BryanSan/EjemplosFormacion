using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para leer los Headers mas comunes y los custom Header del Request  y Response
    /// Para los mas comunes puedes usar las propiedades disponibles para interactuar con ellos
    /// </summary>
    class TestReadHeaderMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestReadHeaderMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestReadHeaderMessageHandler()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
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