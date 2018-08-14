using EjemplosFormacion.WebApi.ParametersBindingAttributes;
using EjemplosFormacion.WebApi.Stubs.Models;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestParameterBindingAttributes
{
    public class TestETagMatchController : ApiController
    {
        public IHttpActionResult TestETagIfMatchAttribute([TestETagIfMatch] TestETagModel etag)
        {
            return Ok();
        }

        public IHttpActionResult TestETagIfNoneMatchAttribute([TestETagIfNoneMatch] TestETagModel etag)
        {
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult TestETagHttpParameterBindingRegisterInServices(TestETagModel etag)
        {
            return Ok();
        }
    }
}