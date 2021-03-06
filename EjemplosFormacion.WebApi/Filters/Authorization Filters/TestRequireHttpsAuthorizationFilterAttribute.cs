﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Authorization Filter usado para verificar que el Request se este realizando un Schema de Https
    /// Si no es asi devuelve un BadRequest hasta que se realize con Http
    /// </summary>
    public class TestRequireHttpsAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;

            // Verificamos si el Request viene con un Schema de Https
            // Si viene con el Schema dejamos el resto a la clase Base para que haga la logica de siempre
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                // Si no es asi significa que estamos en http y construimos un response con un HttpStatusCode de BadRequest para denotar error
                actionContext.Response = request.CreateResponse(HttpStatusCode.Forbidden, "HTTPS Required");
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}