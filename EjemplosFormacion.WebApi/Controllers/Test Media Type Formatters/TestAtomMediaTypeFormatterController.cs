using EjemplosFormacion.WebApi.MediaTypeFormatters;
using System.Collections.Generic;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestMediaTypeFormatters
{
    public class TestAtomMediaTypeFormatterController : ApiController
    {
        public IHttpActionResult TestAtomMediaTypeFormatter()
        {
            var listProducts = new List<TestAtomMediaTypeFormatter.Product>
            {
                new TestAtomMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
                new TestAtomMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
                new TestAtomMediaTypeFormatter.Product { Id = 1, Category = "category", Name = "name", Price = 1000},
            };

            return Ok(listProducts);
        }
    }
}
