using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestCustomRoutesConfiguration
{
    public class TestTypedDirectRouteFactoryController : ApiController
    {
        // Ruta configurada tipadamente sin parametros
        public IHttpActionResult TestTypedDirectRouteFactoryNoParams()
        {
            return Ok();
        }

        // Ruta configurada tipadamente con parametros
        public IHttpActionResult TestTypedDirectRouteFactoryWithParams(int id)
        {
            return Ok(id);
        }
    }
}
