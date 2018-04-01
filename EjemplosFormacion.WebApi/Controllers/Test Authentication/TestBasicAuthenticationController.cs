﻿using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestAuthentication
{
    // Web Api Build in Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado), necesario para que si no tiene credenciales explote
    [Authorize]
    [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
    public class TestBasicAuthenticationController : ApiController
    {
        [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
        public IHttpActionResult TestBasicAuthenticationFilter()
        {
            return Ok();
        }
    }
}
