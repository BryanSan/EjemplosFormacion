using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ExceptionFilters
{
    /// <summary>
    /// Filter attribute para interceptar excepciones que hereda de la clase ExceptionFilterAttribute para evitar tener que implementar los metodos requeridos por la interfaz IExceptionFilter
    /// Se puede usar para sustituir el response con un response mas adecuado segun la logica diga por ejemplo por el tipo de excepcion
    /// </summary>
    class TestExceptionFilterAttribute : ExceptionFilterAttribute
    {
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