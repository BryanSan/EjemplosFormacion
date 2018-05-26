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
        // Test Authorize Filter que extiende el Built In AuthorizeAttribute
        public IHttpActionResult TestAuthorizeExtendedFilter()
        {
            return Ok();
        }

        [TestAuthorizationFilter] // Authorize Attribute
        // Test Authorize Filter vacio que implementa AuthorizationFilterAttribute
        public IHttpActionResult TestAuthorizeFilter()
        {
            return Ok();
        }

        [TestIAuthorizationFilter] // Authorize Attribute
        // Test Authorize Filter vacio que implemena la interfaz IAuthorizationFilter
        public IHttpActionResult TestIAuthorizationFilter()
        {
            return Ok();
        }

        [TestOrderedAuthorizationFilter(Order = 1)] // Authorize Attribute, se ejecuta primero
        [TestOrderedAuthorizationFilter(Order = 2)] // Authorize Attribute, se ejecuta segundo
        // Test Authorize Filter que se ejecuta en Orden
        public IHttpActionResult TestOrderedAuthorizationFilter()
        {
            return Ok();
        }

        [TestRedirectHttpToHttpsFilter]
        public IHttpActionResult TestRedirectHttpToHttpsFilter()
        {
            return Ok();
        }

        [TestRequireHttpsFilter]
        public IHttpActionResult TestRequireHttpsFilter()
        {
            return Ok();
        }
    }
}