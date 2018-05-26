using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Action Filter usado para verificar que el Request se este realizando un Schema de Https
    /// Si no es asi lee el Uri del Request para crear una copia que tenga con Https 
    /// Devuelve un Response con el HttpStatusCode Found y un Header Location con la Uri creada con Https
    /// Esto hace que el Browser o Debugger o lo que sea cuando vea este HttpStatusCode de Foun junto con un Location Header
    /// Realizara una nueva peticion automaticamente a la Uri descrita en el Header Location
    /// Digamos que es como un Auto Redirect
    /// </summary>
    class TestRedirectHttpToHttpsFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;

            // Verificamos si el Request viene con un Schema de Https
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                // Si no es asi significa que estamos en http y construimos un response con un HttpStatusCode de Found
                // Para hacer el redirect a su version de Https
                actionContext.Response = request.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("<p>Html is required</p>");

                // Construimos un UriBuilder con el RequestUri para modificar las propiedades del Uri
                var uriBuilder = new UriBuilder(request.RequestUri);
                uriBuilder.Scheme = Uri.UriSchemeHttps; // Cambiamos el Schema de Http a Https
                uriBuilder.Port = 443; // Cambiamos el puerto si es necesario por el que use el SSL

                // Asignamos al header Location la nueva Uri con el Schema Https 
                // Para que el navegador cuando vea que tiene el HttpStatusCode de Found redireccione a esta Uri que estamos indicando
                // Logrando asi el objetivo de redireccionar de mi Schema Http a Https
                actionContext.Response.Headers.Location = uriBuilder.Uri;
            }
        }
    }
}