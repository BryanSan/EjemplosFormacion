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
    }
}