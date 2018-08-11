using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestBuiltInAttributes
{
    public class TestAcceptVerbsAttributeController : ApiController
    {
        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod GET o POST
        // Es una variante de los tipicos HttpGet y HttpPost
        // EL RESULTADO ES EL MISMO AL USAR CUALQUIERA DE LAS DOS VARIACIONES (AcceptVerbs o HttpMethod)
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult TestAcceptVerbsAttribute()
        {
            return Ok();
        }
    }
}
