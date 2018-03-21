using EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters
{
    /// <summary>
    /// Filter Attribute comun en el que hereda de la clase ActionFilterWithOrderAttribute para dar soporte al orden de ejecucion de Action a traves de la propiedad Order  
    /// Al mismo ActionFilterWithOrderAttribute hereda de ActionFilterAttribute para evitar tener que implementar los metodos requeridos por la interfaz IActionFilter y IFilter
    /// Has override de los metodos que necesites y añade la logica necesaria
    /// Puedes modificar el request, el response o interrumpir el procesamiento
    /// </summary>
    public class TestOrderedActionFilterAttribute : ActionFilterWithOrderAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        public TestOrderedActionFilterAttribute(int order = 0)
        {
            // Especifica el Order que deseas, esto sera leido por el custom Filter Provider para determinar el orden
            Order = order;
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