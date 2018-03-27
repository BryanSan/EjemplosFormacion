using System.Collections.Generic; 
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter para leer Headers tanto del Request como del Response
    /// Los Headers del Request son los mas usado para leer y engloban tanto los mas comunes (los cuales tienen sus propias propiedades para leerlos desde el Request) como los Custom Headers definidos por la aplicacion
    /// Los Headers del Response son menos comunes y tienen menos predefinidos, sin embargo tiene uno que otro, tambien es posible leer los Custom Header
    /// </summary>
    public class TestReadHeaderActionFilter : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // El Request tiene varias propiedades para leer Headers que son comunes, como Accept, Authorization, etc...
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypeHeaderList = actionContext.Request.Headers.Accept;
            MediaTypeWithQualityHeaderValue mediaTypeHeader = mediaTypeHeaderList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptChartSetList = actionContext.Request.Headers.AcceptCharset;
            StringWithQualityHeaderValue acceptChartSet = acceptChartSetList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptEncodingList = actionContext.Request.Headers.AcceptEncoding;
            StringWithQualityHeaderValue acceptEncoding = acceptEncodingList.FirstOrDefault();

            HttpHeaderValueCollection<StringWithQualityHeaderValue> acceptLanguageList = actionContext.Request.Headers.AcceptLanguage;
            StringWithQualityHeaderValue acceptLanguage = acceptLanguageList.FirstOrDefault();

            AuthenticationHeaderValue authenticationHeaderValue = actionContext.Request.Headers.Authorization;

            // Si quieres buscar un Custom header solo buscalo por su Text Key, cuidado con una Excepcion por si el Header no existe
            IEnumerable<string> headerValues;
            var customHeaderText = string.Empty;

            if (actionContext.Request.Headers.TryGetValues("customHeader", out headerValues))
            {
                customHeaderText = headerValues.FirstOrDefault();
            }

            base.OnActionExecuting(actionContext);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // El Response tiene menos Headers comunes
            HttpHeaderValueCollection<string> acceptRangeHeaderList = actionExecutedContext.Response.Headers.AcceptRanges;
            HttpHeaderValueCollection<ProductInfoHeaderValue> serverHeaderList = actionExecutedContext.Response.Headers.Server;

            // Si quieres buscar un Custom header solo buscalo por su Text Key, cuidado con una Excepcion por si el Header no existe
            IEnumerable<string> headerValues;
            var customHeaderText = string.Empty;

            if (actionExecutedContext.Response.Headers.TryGetValues("customHeader", out headerValues))
            {
                customHeaderText = headerValues.FirstOrDefault();
            }

            base.OnActionExecuted(actionExecutedContext);
        }


        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}