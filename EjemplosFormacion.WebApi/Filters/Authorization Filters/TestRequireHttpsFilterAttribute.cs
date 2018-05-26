using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Action Filter usado para verificar que el Request se este realizando un Schema de Https
    /// Si no es asi devuelve un BadRequest hasta que se realize con Http
    /// </summary>
    public class TestRequireHttpsFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;

            // Verificamos si el Request viene con un Schema de Https
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                // Si no es asi significa que estamos en http y construimos un response con un HttpStatusCode de BadRequest para denotar error
                actionContext.Response = request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}