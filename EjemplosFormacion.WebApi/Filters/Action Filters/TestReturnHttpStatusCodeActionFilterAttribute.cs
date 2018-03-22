using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que demuestra como interrumpir el procesamiento de un Request (Sin excepciones) al asignar la propiedad Response a un HttpStatusCode apropiado
    /// Al asignar el response el Web Api para el procesamiento y sale
    /// </summary>
    public class TestReturnHttpStatusCodeActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        public TestReturnHttpStatusCodeActionFilterAttribute()
        {

        }

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
            // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            base.OnActionExecuting(actionContext);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
            // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            base.OnActionExecuted(actionExecutedContext);
        }


        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
            // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
            // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

    }
}