using EjemplosFormacion.WebApi.ActionsFilters.NormalActionFilters;
using EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestActionFilter]
    [TestIActionFilter]
    [TestOrderedActionFilter(Order = 1)]
    public class TestController : ApiController
    {
        public TestController()
        {

        }


        [HttpGet]
        public IHttpActionResult Test()
        {
            return Ok();
        }
    }
}
