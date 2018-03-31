using EjemplosFormacion.WebApi.Filters.AuthorizationFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.AuthorizationFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFilters
{
    [TestExtendedAuthorizeFilter] // Authorize Attribute
    [TestAuthorizationFilter] // Authorize Attribute
    [TestIAuthorizationFilter] // Authorize Attribute
    [TestOrderedAuthorizationFilter(Order = 1)] // Authorize Attribute, se ejecuta primero
    [TestOrderedAuthorizationFilter(Order = 2)] // Authorize Attribute, se ejecuta segundo
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

        [TestOrderedAuthorizationFilter(Order = 1)] // Authorize Attribute, se ejecuta primero
        [TestOrderedAuthorizationFilter(Order = 2)] // Authorize Attribute, se ejecuta segundo
        public IHttpActionResult TestOrderedAuthorizationFilter()
        {
            return Ok();
        }
    }
}