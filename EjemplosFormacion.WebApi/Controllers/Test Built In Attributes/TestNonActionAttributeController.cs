using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestBuiltInAttributes
{
    public class TestNonActionAttributeController : ApiController
    {
        // Attribute que denota que esta operacion aunque cumpla con los requisitos para ser un Action de un Controller 
        // Sera excluida y no podra ser invocada por una peticion Http, usala para evitar que un cliente use este endpoint/action
        [NonAction]
        public IHttpActionResult TestNonActionAttribute()
        {
            return Ok();
        }
    }
}
