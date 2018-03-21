using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestActionFilter] // Action Filter
    [TestIActionFilter] // Action Filter
    [TestOrderedActionFilter(Order = 1)] // Ordered Action Filter

    [TestExceptionFilter] // Excepcion Filter
    [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter
    public class TestController : ApiController
    {
        public TestController()
        {

        }


        [TestActionFilter] // Action Filter
        [TestIActionFilter] // Action Filter
        [TestOrderedActionFilter(Order = 1)] // Ordered Action Filter

        [HttpGet] // Marca el action como solo accesible por una peticion Get 
        [ActionName("actionFilter")] // Nombre de la accion para cual una url debe coincidir (Tiene un custom route en el web api config para leer el nombre del accion)
        public IHttpActionResult TestActionFilter()
        {
            return Ok();
        }

        [TestExceptionFilter] // Excepcion Filter
        [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter

        [HttpGet] // Marca el action como solo accesible por una peticion Get 
        [ActionName("excepcionFilter")] // Nombre de la accion para cual una url debe coincidir (Tiene un custom route en el web api config para leer el nombre del accion)
        public IHttpActionResult TestExcepcionFilter()
        {
            throw new NotImplementedException();
        }
    }
}
