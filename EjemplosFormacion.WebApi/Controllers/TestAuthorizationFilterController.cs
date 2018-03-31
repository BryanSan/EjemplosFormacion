using EjemplosFormacion.WebApi.Filters.AuthorizationFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestExtendedAuthorizeFilter] // Authorize Attribute
    [TestAuthorizationFilter] // Authorize Attribute
    [TestIAuthorizationFilter] // Authorize Attribute
    public class TestAuthorizationFilterController : ApiController
    {
        [TestExtendedAuthorizeFilter] // Authorize Attribute
        public IHttpActionResult TestAuthorizeExtendedFilter()
        {
            return Ok();
        }

        [TestAuthorizationFilter] // Authorize Attribute
        public IHttpActionResult TestAuthorizeFilter()
        {
            return Ok();
        }

        [TestIAuthorizationFilter] // Authorize Attribute
        public IHttpActionResult TestIAuthorizationFilter()
        {
            return Ok();
        }
    }
}