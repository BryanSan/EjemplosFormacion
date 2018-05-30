using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V1
{
    public class TestVersionControllerVersusNameSpaceController : ApiController
    {
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 1");
        }
    }
}
