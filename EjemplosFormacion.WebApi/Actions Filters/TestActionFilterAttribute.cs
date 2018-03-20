using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.ActionsFilters
{
    /// <summary>
    /// Filter Attribute comun en el que hereda de la clase ActionFilterAttribute para evitar tener que implementar los metodos requeridos por la interfaz IActionFilter
    /// Has override de los metodos que necesites y añade la logica necesaria
    /// </summary>
    public class TestActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        public TestActionFilterAttribute()
        {

        }

        //http://www.tutorialsteacher.com/webapi/web-api-filters
        //https://msdn.microsoft.com/en-us/library/system.web.http.filters(v=vs.118).aspx
        //https://www.strathweb.com/2013/11/asynchronous-action-filters-asp-net-web-api/

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