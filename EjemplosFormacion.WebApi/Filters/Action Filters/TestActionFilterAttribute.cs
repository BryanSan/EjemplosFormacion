using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Filter Attribute comun en el que hereda de la clase ActionFilterAttribute para evitar tener que implementar los metodos requeridos por la interfaz IActionFilter y IFilter
    /// Has override de los metodos que necesites y añade la logica necesaria
    /// Puedes modificar el request, el response o interrumpir el procesamiento
    /// </summary>
    public class TestActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        public TestActionFilterAttribute()
        {
            
        }

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            // De esta manera puedes modificar valores obtenidos de los datos del Routing
            if (actionContext.RequestContext.RouteData.Values.TryGetValue("id", out _))
            {   
                actionContext.RequestContext.RouteData.Values["id"] = 0;
            }

            // De esta manera puedes modificar los valores del Request, tanto los valores del Body como el Url
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
            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            base.OnActionExecuted(actionExecutedContext);
        }


        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica asincronica
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

    }
}