using System;
using System.Net;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestServices
{
    public class TestExceptionHandlerController : ApiController
    {
        // Demostracion de como un ExceptionHandler maneja las Excepciones si no fue handleada por el Action o un ExceptionFilter
        public IHttpActionResult TestExceptionHandler()
        {
            throw new NotImplementedException();
        }

        // Demostracion de como un ExceptionHandler no hace Handled a Excepciones del tipo HttpResponseException 
        // Esto es un caso especial
        public IHttpActionResult TestExceptionHandlerNotHandleResponseException()
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        // Demostracion de como un ExceptionLogger Loggea las Excepciones indiferentemente si no fue handleada por un ExceptionFilter o ExceptionHandler
        // Si la Excepcion fue handleada en el Action no se Loggeara
        public IHttpActionResult TestExceptionLogger()
        {
            throw new NotImplementedException();
        }

        // Demostracion de como un ExceptionLogger no hace Logger a Excepciones del tipo HttpResponseException 
        // Esto es un caso especial
        public IHttpActionResult TestExceptionLoggerNotLogResponseException()
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}
