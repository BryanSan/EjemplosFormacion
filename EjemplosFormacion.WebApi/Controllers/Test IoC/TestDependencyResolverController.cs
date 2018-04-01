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

        public IHttpActionResult TestDependencyResolver()
        {
            return Ok();
        }
    }
}
