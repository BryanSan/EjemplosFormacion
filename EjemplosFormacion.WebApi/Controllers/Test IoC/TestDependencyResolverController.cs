using EjemplosFormacion.WebApi.Stubs.Abstract;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestIoC
{
    public class TestDependencyResolverController : ApiController
    {
        readonly ITestDependency _dependency;

        public TestDependencyResolverController(ITestDependency dependency)
        {
            _dependency = dependency;
        }

        // Test Dependency Resolver para revisar que la dependencia sea resuelta
        public IHttpActionResult TestDependencyResolver()
        {
            if (_dependency != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Dependencia null!.");
            }
        }
    }
}
