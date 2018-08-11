using EjemplosFormacion.WebApi.ControllerConfigurations;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestControllerConfiguration
{
    // Aplicas el Controller Configuration que quieres para este Controller usandolo como un Attribute en la clase
    // El Controller Configuration hara override solo en la configuracion de este Controller segun sea la logica del Controller Configuration
    // Puedes hacerle override a 
    //     Media-type formatters
    //     Parameter binding rules
    //     Services
    [TestControllerConfiguration]
    public class TestControllerConfigurationController : ApiController
    {
        public IHttpActionResult TestControllerConfiguration()
        {
            return Ok();
        }
    }
}
