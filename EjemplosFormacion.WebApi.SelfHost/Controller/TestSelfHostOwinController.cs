using EjemplosFormacion.WebApi.SelfHost.Model;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.SelfHost.Controller
{
    public class TestSelfHostOwinController : ApiController
    {
        public HttpResponseMessage TestOwin()
        {
            TestModel news = new TestModel() { Summary = "The world is falling apart." };
            return Request.CreateResponse<TestModel>(HttpStatusCode.OK, news);
        }
    }
}