using EjemplosFormacion.WebApi.MediaTypeFormatters;
using System.Collections.Generic;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestMediaTypeFormatters
{
    public class TestCSVBufferedMediaTypeFormatterController : ApiController
    {
        // Metodo para retornar entidades en formato CSV con ayuda de un Custom Media Type Formatter
        public IHttpActionResult TestCSVBufferedMediaTypeFormatter()
        {
            var listProducts = new List<TestCSVBufferedMediaTypeFormatter.Product>
            {
                new TestCSVBufferedMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
                new TestCSVBufferedMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
                new TestCSVBufferedMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
            };

            return Ok(listProducts);
        }
    }
}
