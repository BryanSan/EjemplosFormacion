using System;
using System.Diagnostics;
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
    /// Filter Attribute comun en el que implementa la interfaz que lo hace un action filter 
    /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
    /// Puedes modificar el request, el response o interrumpir el procesamiento
    /// </summary>
    public class TestIActionFilterAttribute : FilterAttribute, IActionFilter
    {
        /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Trace.WriteLine(string.Format("Action Method {0} executing at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");

            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            // Antes de ejecutar el Action
            var result = await continuation();
            // Despues de ejecutar el Action

            // Para devolver un response mensaje y cortar el procesamiento del request puedes hacer estas dos cosas
            {
                // Puedes asignar el reponse del ActionContext y devolvera el response al terminar este filtro cortando mas procesamiento del pipeline de la request
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                // Puedes tirar una excepcion con un response exception, el codigo y el error a mostrar
                // Es mas elegante asignar el response que tirar una excepcion, recuerda que excepciones penalizan al performance
                // throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error"));
            }

            Trace.WriteLine(string.Format("Action Method {0} executed at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");

            return result;

        }
    }
}