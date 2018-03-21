using EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters
{
    /// <summary>
    /// Filter attribute para interceptar excepciones que hereda de la clase ExceptionFilterWithOrderAttribute para dar soporte al ordenamiento
    /// Al mismo tiempo la clase padre implementa los metodos requeridos por la interfaz IExceptionFilter para evitar tener que dar una implementacion
    /// Se puede usar para sustituir el response con un response mas adecuado segun la logica diga por ejemplo por el tipo de excepcion
    /// </summary>
    public class TestOrderedExceptionFilterAttribute : ExceptionFilterWithOrderAttribute
    {
        public TestOrderedExceptionFilterAttribute(int order = 0)
        {
            Order = order;
        }

        // Mayormente el metodo se tratara de revisar que excepcion tiene la action y dependiendo del tipo de Excepcion crear el response
        // Con este puedes usar metodos sincronicos
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
        }

        // Mayormente el metodo se tratara de revisar que excepcion tiene la action y dependiendo del tipo de Excepcion crear el response
        // Con este puedes usar metodos asincronicos
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}