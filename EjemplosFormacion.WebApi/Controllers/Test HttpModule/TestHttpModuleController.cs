using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpModule
{
    public class TestHttpModuleController : ApiController
    {
        // Demuestra el uso de un HttpModule
        public IHttpActionResult TestHttpModule()
        {
            return Ok();
        }
    }
}
