using EjemplosFormacion.WebApi.Stubs.Models;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestMessagingHandler
{
    public class TestMessagingHandlerController : ApiController
    {
        // Test Message Handler vacio
        public IHttpActionResult TestMessageHandler()
        {
            return Ok();
        }

        // Test Message Handler vacio creado a partir de una Factory y asignado a esta ruta en especifico
        public IHttpActionResult TestWithMessagingHandlerWithFactory()
        {
            return Ok();
        }

        // Test Message Handler vacio creado especificamente para esta ruta sin ningun otro Message Handler que le continue
        public IHttpActionResult TestMessagingHandlerRouteSpecificNoChain()
        {
            return Ok();
        }

        // Test Message Handler vacio creado especificamente para esta ruta que le sigue otro Message Handler 
        public IHttpActionResult TestMessagingHandlerRouteSpecificYesChain()
        {
            return Ok();
        }

        // Test Message Handler vacio creado especificamente para esta ruta que retorna una Response, 
        // El Request jamas llega a este metodo ya que el Message Handler crea la Response e interrumpe el procesamiento
        public IHttpActionResult TestReturnResponseMessageHandler()
        {
            return Ok();
        }

        // Test Message Handler creado especificamente para esta ruta que revisa por el Header X-HTTP-Method-Override
        // Y actualiza el metodo (HttpVerb) que se uso en el Request con el especificado en el Header X-HTTP-Method-Override
        public IHttpActionResult TestMethodOverrideHeaderMessageHandler()
        {
            return Ok();
        }

        // Test Message Handler creado especificamente para esta ruta que agrega un custom header al Request y Response
        public IHttpActionResult TestAddHeaderMessageHandler()
        {
            return Ok();
        }

        // Test Message Handler creado especificamente para esta ruta que lee valores del Query String
        public IHttpActionResult TestReadQueryStringMessagingHandler()
        {
            return Ok();
        }

        // Test Message Handler creado especificamente para esta ruta que lee y asigna Cookies 
        public IHttpActionResult TestCookiesMessageHandler()
        {
            return Ok();
        }

        // Test Message Handler creado especificamente para esta ruta que lee Headers del Request y Response
        public IHttpActionResult TestReadHeaderMessageHandler()
        {
            return Ok();
        }

        // Recordar enviar las credenciales de Authorize Basic usuario:clave
        [Authorize]
        // Test Message Handler creado especificamente para esta ruta para autenticar un Request mediante el Schema Basic
        public IHttpActionResult TestBasicAuthenticatonMessageHandler()
        {
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult TestBasicAuthenticatonMessageHandlerShowDialogBox()
        {
            return Ok();
        }

        // Recordar enviar el Request encriptado
        // Mensaje para testear --->>> vAgrUyJeVaA5RAyNHh3yOtTW+z/HR4yL+euHWG9TuM1N2A4ACl0Z34OAGuU/qdqJ
        // Test Message Handler creado especificamente para esta ruta que Desencripta el Request y Encripta el Response
        public IHttpActionResult TestJsonEncrypterMessageHandler(TestModel testModel)
        {
            return Ok(testModel);
        }

        // Test Message Handler usado para leer el Client Certificate que viene con el Request
        public IHttpActionResult TestReadClientCertificateMessageHandler()
        {
            return Ok();
        }

    }
}