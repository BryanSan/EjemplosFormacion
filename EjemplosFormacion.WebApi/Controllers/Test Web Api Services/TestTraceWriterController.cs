using System.Web.Http;
using System.Web.Http.Tracing;

namespace EjemplosFormacion.WebApi.Controllers.TestWebApi_Services
{
    public class TestTraceWriterController : ApiController
    {
        public IHttpActionResult TestTraceWriter()
        {
            // El metodo GetTraceWriter es una manera de recuperar el ITraceWriter configurado (si es que hay uno ya que por default no hay ninguno, debes dar el tuyo propio)
            // Es un Extension Method para el HttpConfiguration.Services (ServicesContainer)
            ITraceWriter traceWriter =  Configuration.Services.GetTraceWriter();
            traceWriter.Debug(Request, "MyCategory", "Mensaje");

            return Ok();
        }
    }
}
