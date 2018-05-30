using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V1
{
    public class TestVersionControllerVersusNamespaceController : ApiController
    {
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 1");
        }
    }
}
