using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestExceptionFilter] // Excepcion Filter
    [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter
    public class TestExceptionFilterController : ApiController
    {
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
