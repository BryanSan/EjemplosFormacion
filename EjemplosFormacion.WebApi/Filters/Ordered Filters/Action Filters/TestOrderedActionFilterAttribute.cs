using EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters
{
    /// <summary>
    /// Action Filter Attribute comun en el que hereda de la clase ActionFilterWithOrderAttribute para dar soporte al orden de ejecucion de Action a traves de la propiedad Order  
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
            Trace.WriteLine(string.Format("TestOrderedActionFilter with Order {0} executing OnActionExecuting at {1}", Order, DateTime.Now.ToShortDateString()), "Web API Logs");

            base.OnActionExecuting(actionContext);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Trace.WriteLine(string.Format("TestOrderedActionFilter with Order {0} executing OnActionExecuted at {1}", Order, DateTime.Now.ToShortDateString()), "Web API Logs");

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