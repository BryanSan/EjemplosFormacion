using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que demuestra como se interrumpe el procesamiento del Request al lanzar una Excepcion y devolver un HttpStatusCode dentro de ella
    /// Las excepciones penalizan el performance, es mas apropiado asignar el response con el HttpStatusCode y devolver sin excepcion
    /// Al asignar el response el Web Api interrumpe el procesamiento y sale
    /// </summary>
    public class TestReturnExceptionActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        public TestReturnExceptionActionFilterAttribute()
        {

        }

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
            // Es mas elegante asignar el response con un HttpStatusCode apropiado que tirar una excepcion, recuerda que excepciones penalizan al performance
            throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
            // Es mas elegante asignar el response con un HttpStatusCode apropiado que tirar una excepcion, recuerda que excepciones penalizan al performance
            throw new HttpResponseException(actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
        }


        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
            // Es mas elegante asignar el response con un HttpStatusCode apropiado que tirar una excepcion, recuerda que excepciones penalizan al performance
            throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
            // Es mas elegante asignar el response con un HttpStatusCode apropiado que tirar una excepcion, recuerda que excepciones penalizan al performance
            throw new HttpResponseException(actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
        }

    }
}