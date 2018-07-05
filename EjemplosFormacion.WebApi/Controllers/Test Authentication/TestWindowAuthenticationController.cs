using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestAuthentication
{
    [Authorize]
    public class TestWindowAuthenticationController : ApiController
    {
        [HttpGet]
        public IHttpActionResult TestWindowAuthentication()
        {
            return Ok();
        }
    }
}
