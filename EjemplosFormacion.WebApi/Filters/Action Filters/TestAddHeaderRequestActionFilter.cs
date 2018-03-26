using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    public class TestAddHeaderRequestActionFilter : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // De esta manera puedes Agregar un Header
            actionContext.Request.Headers.Add("customHeader", "custom value date time");

            // El Request tiene varias propiedades para leer Headers que son comunes, como Accept y Authorization
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypeHeader = actionContext.Request.Headers.Accept;
            MediaTypeWithQualityHeaderValue mediaTypeText = mediaTypeHeader.FirstOrDefault();

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