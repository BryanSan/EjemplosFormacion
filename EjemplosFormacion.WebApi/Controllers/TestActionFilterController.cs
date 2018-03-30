using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestActionFilter] // Action Filter - Al no ser un Ordered Action Filter se ejecuta luego de los Ordered Filter (Ver implementacion Filter Provider)
    [TestIActionFilter] // Action Filter - Al no ser un Ordered Action Filter se ejecuta luego de los Ordered Filter (Ver implementacion Filter Provider)
    [TestOrderedActionFilter(Order = 1)] // Ordered Action Filter - Primero en ejecutar
    [TestOrderedActionFilter(Order = 2)] // Ordered Action Filter - Segundo en ejecutar
    [TestOrderedActionFilter(Order = 3)] // Ordered Action Filter - Tercero en ejecutar
    public class TestActionFilterController : ApiController
    {
        [TestActionFilter] // Action Filter
        [TestIActionFilter] // Action Filter
        public IHttpActionResult TestActionFilter()
        {
            return Ok();
        }

        [TestActionFilter] // Action Filter - Al no ser un Ordered Action Filter se ejecuta luego de los Ordered Filter (Ver implementacion Filter Provider)
        [TestOrderedActionFilter(Order = 1)] // Ordered Action Filter - Primero en ejecutar
        [TestOrderedActionFilter(Order = 2)] // Ordered Action Filter - Segundo en ejecutar
        [TestOrderedActionFilter(Order = 3)] // Ordered Action Filter - Tercero en ejecutar
        public IHttpActionResult TestOrderedActionFilter()
        {
            return Ok();
        }

        // Interrumpe el procesamiento, genera y devuelve una excepcion con un HttpStatusCode de BadRequest (Mejor devuelve un HttpStatusCode apropiado que una excepcion)
        [TestReturnExceptionActionFilter]
        public IHttpActionResult TestReturnExceptionActionFilter()
        {
            // Nunca entra aqui ya que el Filter interrumpe el procesamiento
            return Ok();
        }

        // Interrumpe el procesamiento y devuelve un HttpStatusCode de BadRequest sin generar una excepcion
        [TestReturnHttpStatusCodeActionFilter]
        public IHttpActionResult TestReturnHttpStatusCodeActionFilter()
        {
            // Nunca entra aqui ya que el Filter interrumpe el procesamiento
            return Ok();
        }

        // Edita el valor id que venga en la request por Routing (Url) 
        [TestEditRequestActionFilter]
        public IHttpActionResult TestEditRequestActionFilterFromUri([FromUri] int id)
        {
            return Ok(id);
        }

        // Edit el valor id que venga en la request en el Body
        [TestEditRequestActionFilter]
        public IHttpActionResult TestEditRequestActionFilterFromBody([FromBody] int id)
        {
            return Ok(id);
        }

        // Edita el valor devuelto en el response
        [TestEditResponseActionFilter]
        public IHttpActionResult TestEditResponseActionFilter()
        {
            return Ok(10);
        }

        // Lee los header del request recibido, tanto los mas comunes como los custom definidos para la aplicacion
        [TestReadHeaderActionFilter]
        public IHttpActionResult TestReadHeaderRequestActionFilter()
        {
            return Ok();
        }

        // Añade headers al Request y Response
        [TestAddHeaderRequestActionFilter]
        public IHttpActionResult TestAddHeaderRequestActionFilter()
        {
            return Ok();
        }
    }
}