using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpModule
{
    public class TestHttpModuleController : ApiController
    {
        public IHttpActionResult TestHttpModule()
        {
            return Ok();
        }
    }
}
