using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFilters
{
    // Web Api Build in Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado), necesario para que si no tiene credenciales explote
    [Authorize] 
    [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
    public class TestAuthenticationFilterController : ApiController
    {
        // Web Api Build in Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado), necesario para que si no tiene credenciales explote
        [Authorize]
        [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
        // Test de Authentication Filter para validar un Request contra el Schema Basic
        public IHttpActionResult TestBasicAuthenticationFilter()
        {
            return Ok();
        }
    }
}