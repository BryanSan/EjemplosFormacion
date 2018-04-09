using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using EjemplosFormacion.WebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFilters
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
        // Test de Action Filter vacios
        public IHttpActionResult TestActionFilter()
        {
            return Ok();
        }

        [TestActionFilter] // Action Filter - Al no ser un Ordered Action Filter se ejecuta luego de los Ordered Filter (Ver implementacion Filter Provider)
        [TestOrderedActionFilter(Order = 1)] // Ordered Action Filter - Primero en ejecutar
        [TestOrderedActionFilter(Order = 2)] // Ordered Action Filter - Segundo en ejecutar
        [TestOrderedActionFilter(Order = 3)] // Ordered Action Filter - Tercero en ejecutar
        // Test de Action Filter vacios y con Orden
        public IHttpActionResult TestOrderedActionFilter()
        {
            return Ok();
        }

        [TestReturnExceptionActionFilter]
        // Test de Action Filter que interrumpe el procesamiento, genera y devuelve una excepcion con un HttpStatusCode de BadRequest (Mejor devuelve un HttpStatusCode apropiado que una excepcion)
        public IHttpActionResult TestReturnExceptionActionFilter()
        {
            // Nunca entra aqui ya que el Filter interrumpe el procesamiento
            return Ok();
        }

        [TestReturnHttpStatusCodeResponseActionFilter]
        // Test de Action Filter que interrumpe el procesamiento y devuelve un HttpStatusCode de BadRequest sin generar una excepcion
        public IHttpActionResult TestReturnHttpStatusCodeResponseActionFilter()
        {
            // Nunca entra aqui ya que el Filter interrumpe el procesamiento
            return Ok();
        }

        // Edita el valor id que venga en la request por Routing (Url) 
        [TestEditRequestActionFilter]
        // Test de Action Filter que edita un valor del Request que venga de la Uri
        public IHttpActionResult TestEditRequestActionFilterFromUri([FromUri] int id)
        {
            return Ok(id);
        }

        // Edit el valor id que venga en la request en el Body
        [TestEditRequestActionFilter]
        // Test de Action Filter que edita un valor del Request que venga en el Body
        public IHttpActionResult TestEditRequestActionFilterFromBody([FromBody] int id)
        {
            return Ok(id);
        }

        // Edita el valor devuelto en el response de este Action
        [TestEditResponseActionFilter]
        // Test de Action Filter que edita el valor devuelto en el Response
        public IHttpActionResult TestEditResponseActionFilter()
        {
            return Ok(10);
        }

        [TestReadHeaderActionFilter]
        // Test de Action Filter que lee los header del request recibido, tanto los mas comunes como los custom definidos para la aplicacion
        public IHttpActionResult TestReadHeaderRequestActionFilter()
        {
            return Ok();
        }

        [TestAddHeaderActionFilter]
        // Test de Action Filter que añade headers al Request y Response
        public IHttpActionResult TestAddHeaderRequestActionFilter()
        {
            return Ok();
        }

        [TestValidationModelStateActionFilter]
        // Test que valida los Data Annotation y devuelve error si no se cumplen las reglas de validacion
        public IHttpActionResult TestValidationModelStateActionFilter(TestModel testModel)
        {
            return Ok();
        }

        [TestRequiredParameterActionFilter]
        // Test que valida que los parametros marcados como [Required] existan, no sean nulos y que no no esten vacios (para las listas)
        public IHttpActionResult TestRequiredParameterActionFilter([Required] TestModel testModel)
        {
            return Ok();
        }

        [TestLoggingActionFilter]
        // Test que Loggea la ejecucion del Action
        public IHttpActionResult TestLoggingActionFilter()
        {
            return Ok();
        }

        [TestWithDependencyActionFilter]
        // Test que valida que la Dependencia del Action Filter sea resulta, como es un attributo no sirve el Construction Injection y se hace Property Injection
        public IHttpActionResult TestWithDependencyActionFilter()
        {
            return Ok();
        }
    }
}