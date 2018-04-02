using EjemplosFormacion.WebApi.Models;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestMessagingHandler
{
    public class TestMessagingHandlerController : ApiController
    {

        public IHttpActionResult TestMessageHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestWithMessagingHandlerWithFactory()
        {
            return Ok();
        }

        public IHttpActionResult TestMessagingHandlerRouteSpecificNoChain()
        {
            return Ok();
        }

        public IHttpActionResult TestMessagingHandlerRouteSpecificYesChain()
        {
            return Ok();
        }

        public IHttpActionResult TestReturnResponseMessageHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestMethodOverrideHeaderMessageHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestAddHeaderMessageHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestReadQueryStringMessagingHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestCookiesMessageHandler()
        {
            return Ok();
        }

        public IHttpActionResult TestReadHeaderMessageHandler()
        {
            return Ok();
        }

        // Recordar enviar las credenciales de Authorize Basic usuario:clave
        [Authorize]
        public IHttpActionResult TestBasicAuthenticatonMessageHandler()
        {
            return Ok();
        }

        // Recordar enviar el Request encriptado
        // vAgrUyJeVaA5RAyNHh3yOtTW+z/HR4yL+euHWG9TuM1N2A4ACl0Z34OAGuU/qdqJ
        public IHttpActionResult TestJsonEncrypterMessageHandler(TestModel testModel)
        {
            return Ok(testModel);
        }
    }
}