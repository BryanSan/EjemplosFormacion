using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que demuestra como se edita los valores del Request que vengan tanto del Routing (Url), como del Body - Probar con parametro "id"
    /// </summary>
    public class TestEditRequestActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // De esta manera puedes modificar los valores obtenidos SOLO de los datos del Routing (Url)
            if (actionContext.RequestContext.RouteData.Values.TryGetValue("id", out _))
            {
                actionContext.RequestContext.RouteData.Values["id"] = 0;
            }

            // De esta manera puedes modificar los valores del Request, TANTO los valores del Body como el Url
            // Todos los valores del Request que sean usados en los parametros del action seran bindeados y apareceran en el diccionario
            // Todos los valores del Request que no sean usados en los parametros del action no seran bindeados ya que seran ignorados y por lo tanto no apareceran en el diccionario
            if (actionContext.ActionArguments.TryGetValue("id", out _))
            {
                actionContext.ActionArguments["id"] = 0;
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