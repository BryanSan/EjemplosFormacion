using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V2
{
    public class TestVersionControllerVersusNameSpaceV2Controller : ApiController
    {
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 2");
        }
    }
}
