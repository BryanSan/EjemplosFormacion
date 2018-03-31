using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Extend AuthorizationFilterAttribute to perform synchronous authorization logic that is not necessarily based on the current user or role.
    /// </summary>
    public class TestAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        // Logica para definir tus autorizaciones en manera sincronica
        // Este es el metodo llamado por Web Api para ver si esta autorizado o no (sincrono)
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Si no esta autorizado devolvemos el error
            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }

            base.OnAuthorization(actionContext);
        }

        // Logica para definir tus autorizaciones en manera asincronica
        // Este es el metodo llamado por Web Api para ver si esta autorizado o no (asincrono)
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Si no esta autorizado devolvemos el error
            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        // Logica para manejar las request que esten desautorizadas
        // En este caso se crea el response a devolver con el error personalizado
        // Este metodo no es llamado por Web Api es un metodo de ayuda 
        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("No Autorizado!."));
        }

        // Logica para definir si esta autorizada el Request o no
        // Aqui consultas o haces lo que necesites para que digas mediante un boolean si esta autorizado o no
        // Este metodo no es llamado por Web Api es un metodo de ayuda 
        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            return true;
        }
    }
}