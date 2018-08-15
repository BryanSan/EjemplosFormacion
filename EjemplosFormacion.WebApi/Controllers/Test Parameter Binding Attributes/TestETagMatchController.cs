using EjemplosFormacion.WebApi.ParametersBindingAttributes;
using EjemplosFormacion.WebApi.Stubs.Models;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestParameterBindingAttributes
{
    public class TestETagMatchController : ApiController
    {
        // Metodo para bindear este parametro con el header If-Match si viene y si esta bien formateado, si no lo esta llegara nulo
        // Web Api sabra que esto tiene bindeado especial ya que tiene un ParameterBindingAttribute asociado
        public IHttpActionResult TestETagIfMatchAttribute([TestETagIfMatch] TestETagModel etag)
        {
            return Ok();
        }

        // Metodo para bindear este parametro con el header If-NoneMatch si viene y si esta bien formateado, si no lo esta llegara nulo
        // Web Api sabra que esto tiene bindeado especial ya que tiene un ParameterBindingAttribute asociado
        public IHttpActionResult TestETagIfNoneMatchAttribute([TestETagIfNoneMatch] TestETagModel etag)
        {
            return Ok();
        }

        // Metodo para bindear este parametro con el header If-NoneMatch si viene y si esta bien formateado, si no lo esta llegara nulo
        // Web Api sabra que esto tiene bindeado especial ya que esta registrado en el WebApiConfig 
        // Que todas las con el Parameter Type TestETagModel y que sean llamadas con el metodo HttpPost usaran un Custom ParameterBindingAttribute
        public IHttpActionResult TestETagHttpParameterBindingRegisterInServices(TestETagModel etag)
        {
            return Ok();
        }
    }
}