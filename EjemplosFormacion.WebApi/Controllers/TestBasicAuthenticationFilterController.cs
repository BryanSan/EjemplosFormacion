using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
    [Authorize] // Requiere que el Request este autenticado (con un IPrincipal asignado)
    public class TestBasicAuthenticationFilterController : ApiController
    {
        [TestBasicAuthenticationFilter] // Authentication Filter with Basic Schema
        [Authorize] // Requiere que el Request este autenticado (con un IPrincipal asignado)
        public IHttpActionResult TestBasicAuthenticationFilter()
        {
            return Ok();
        }
    }
}