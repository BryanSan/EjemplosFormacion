using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestServices
{
    public class TestExceptionHandlerController : ApiController
    {
        public IHttpActionResult TestExceptionHandler()
        {
            throw new NotImplementedException();
        }
    }
}
