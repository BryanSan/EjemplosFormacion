using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.ActionsFilters.NormalActionFilters
{
    /// <summary>
    /// Filter Attribute comun en el que implementa la interfaz que lo hace un action filter 
    /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
    /// </summary>
    public class TestIActionFilterAttribute : FilterAttribute, IActionFilter
    {
        /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Trace.WriteLine(string.Format("Action Method {0} executing at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");
            
            // Antes de ejecutar el Action
            var result = await continuation();
            // Despues de ejecutar el Action

            Trace.WriteLine(string.Format("Action Method {0} executed at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");

            return result;

        }
    }
}