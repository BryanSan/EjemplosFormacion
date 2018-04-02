using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFilters
{
    [TestExceptionFilter] // Excepcion Filter
    [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter - Primero en Ejecutar
    [TestOrderedExceptionFilter(Order = 2)] // Ordered Excepcion Filter - Segundo en Ejecutar
    public class TestExceptionFilterController : ApiController
    {
        [TestExceptionFilter] // Excepcion Filter
        [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter - Primero en Ejecutar
        [TestOrderedExceptionFilter(Order = 2)] // Ordered Excepcion Filter - Segundo en Ejecutar
        // Test Exception Filter que crea un Response personalizada para este tipo de Excepcion
        public IHttpActionResult TestExcepcionFilter()
        {
            throw new NotImplementedException();
        }
    }
}
