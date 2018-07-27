using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ExceptionFilters
{
    class TestIExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        // Mayormente el metodo se tratara de revisar que excepcion tiene la action y dependiendo del tipo de Excepcion crear el response
        // Con este puedes usar metodos asincronicos
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            return Task.CompletedTask;
        }
    }
}