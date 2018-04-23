using EjemplosFormacion.WebApi.ActionResults;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace EjemplosFormacion.WebApi.ExceptionHandlers
{
    /// <summary>
    /// Exception handlers are the solution for customizing all possible responses to unhandled exceptions caught by Web API.
    /// Se usa para ser hacer handler a una Exception que no ha sido atendida por el Action o un Exception Filter, devolver un Response adecuado o cualquier otro tipo de procesamiento
    /// Las excepciones del tipo HttpResponseException son un caso especial y no llamaran al ExceptionHandler
    /// </summary>
    class TestExceptionHandler : IExceptionHandler
    {
        // Metodo de contrato del Exception Handler que se debe implementar la logica para hacer Handle a la Excepcion
        // Aqui puedes revisar la informacion del ExceptionHandlerContext para obtener toda la informacion acerca de la excepcion y Handlear segun sea el caso
        public virtual Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Result = new TextPlainErrorActionResult(context.ExceptionContext.Request, "Oops! Sorry! Something went wrong. Please contact support@contoso.com so we can try to fix it." + Environment.NewLine + context.ExceptionContext.Exception.Message);

            return Task.FromResult(0);
        }
    }
}