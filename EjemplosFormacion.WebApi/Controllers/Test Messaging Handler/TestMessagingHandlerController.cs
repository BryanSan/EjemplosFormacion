using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestMessagingHandler
{
    public class TestMessagingHandlerController : ApiController
    {
        public IHttpActionResult TestMessageHandler()
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

        [Authorize]
        public IHttpActionResult TestBasicAuthenticatonMessageHandler()
        {
            return Ok();
        }
    }
}