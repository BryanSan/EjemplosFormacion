using EjemplosFormacion.WebApi.Filters.OverrideFilters;
using System.Web.Http;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Controllers.TestFilters
{
    [Authorize]
    // Hace override al Filter Authorize Global
    [TestOverrideFilter(typeof(IAuthorizationFilter))] // Override Filter
    public class TestOverrideFilterController : ApiController
    {
        // Hace override al Filter Authorize en el Controller
        [TestOverrideFilter(typeof(IAuthorizationFilter))] // Override Filter
        // Test Override Filter que invalida la ejecucion (no se ejecutan) de los Filter del tipo IAuthorizationFilter
        public IHttpActionResult TestOverrideFilter()
        {
            return Ok();
        }
    }
}
