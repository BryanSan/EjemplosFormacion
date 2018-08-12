using EjemplosFormacion.WebApi.TypeConverters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestTypeConverters
{
    public class TestTypeConverterController : ApiController
    {
        // You can make Web API treat a class as a simple type(so that Web API will try to bind it from the URI) by creating a TypeConverter and providing a string conversion.
        // http://localhost:6719/api/TestTypeConverter/TestTypeConverter?location=47.678558,-122.130989
        // Metodo que usa un Custom Type Converter para hallar el valor de un parametro, en este caso location
        // En este caso obtendra el valor de un parametro del Query String, separandolo por "," y hallando el valor para todas sus propiedades
        public IHttpActionResult TestTypeConverter(TestTypeConverter.GeoPoint location)
        {
            return Ok();
        }
    }
}