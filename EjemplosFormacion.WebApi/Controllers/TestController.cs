using EjemplosFormacion.WebApi.ActionsFilters.NormalActionFilters;
using EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters.NormalActionFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestActionFilter] // Normal Action Filter
    [TestIActionFilter] // Normal Action Filter
    [TestOrderedActionFilter(Order = 1)] // Normal Ordered Action Filter
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
