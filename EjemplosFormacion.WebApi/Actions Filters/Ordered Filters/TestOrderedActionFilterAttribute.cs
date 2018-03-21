using EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters.Infraestructure;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters
{
    /// <summary>
    /// Filter Attribute comun en el que hereda de la clase ActionFilterWithOrderAttribute para dar soporte al orden de ejecucion de Action a traves de la propiedad Order  
    /// Al mismo ActionFilterWithOrderAttribute hereda de ActionFilterAttribute para evitar tener que implementar los metodos requeridos por la interfaz IActionFilter y IFilter
    /// Has override de los metodos que necesites y añade la logica necesaria
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
            base.OnActionExecuting(actionContext);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
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