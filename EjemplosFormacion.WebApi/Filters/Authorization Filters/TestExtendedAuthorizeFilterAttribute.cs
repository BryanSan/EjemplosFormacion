using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Extend of AuthorizeAttribute to perform authorization logic based on the current user and the user's roles.
    /// </summary>
    public class TestExtendedAuthorizeFilterAttribute : AuthorizeAttribute
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
        // Este metodo no es llamado por Web Api es un metodo de ayuda que viene con el AuthorizeAttribute
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("No Autorizado!."));
            base.HandleUnauthorizedRequest(actionContext);
        }

        // Logica para definir si esta autorizada el Request o no
        // Aqui consultas o haces lo que necesites para que digas mediante un boolean si esta autorizado o no
        // Este metodo no es llamado por Web Api es un metodo de ayuda que viene con el AuthorizeAttribute
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }

    }
}