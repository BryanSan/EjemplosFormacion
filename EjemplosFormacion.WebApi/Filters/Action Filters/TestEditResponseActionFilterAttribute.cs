using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que demuestra como se edita los valores del Response - Probar con parametro valores del tipo int
    /// </summary>
    class TestEditResponseActionFilterAttribute : ActionFilterAttribute
    {
        // Para permitir el mismo filtro varias veces (Devuelve lo que necesites)
        public override bool AllowMultiple => base.AllowMultiple;

        // Se ejecuta antes de entrar a ejecutar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        // Se ejecuta al finalizar el Action en el Controller, usalo para logica sincronica
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Puedes inspeccionar el Content tambien casteandolo a un Type apropiado y modificar sus valores
            ObjectContent objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                Type type = objectContent.ObjectType; //type of the returned object
                object value = objectContent.Value; //holding the returned value
                
                if (value is int)
                {
                    // Asignaselo al objectContent.Value para persistir los cambios
                    objectContent.Value = 0;
                }
            }

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