using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter Attribute comun en el que implementa la interfaz que lo hace un action filter 
    /// Es mas facil heredar de ActionFilterAttribute y hacerle override a los metodos correspondientes que implementar la interfaz IActionFilter directamente
    /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
    /// Puedes modificar el request, el response o interrumpir el procesamiento (Ver otros ActionFilter que demuestran como se hace esto)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class TestIActionFilterAttribute : FilterAttribute, IActionFilter
    {
        /// El metodo se ejecuta antes que la action sea ejecutada, debes awaitear el parametro "continuation" para ejecutar operaciones luego que le action halla sido ejecutada
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Trace.WriteLine(string.Format("ITestActionFilter - Action Method {0} executing at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");

            // Antes de ejecutar el Action
            var result = await continuation();
            // Despues de ejecutar el Action

            Trace.WriteLine(string.Format("ITestActionFilter - Action Method {0} executed at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");

            return result;

        }
    }
}