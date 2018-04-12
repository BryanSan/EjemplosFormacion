using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestBuiltInAttributes
{
    public class TestHttpVerbAttributeController : ApiController
    {
        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Get
        [HttpGet]
        public IHttpActionResult GetWithHttpVerbAttribute()
        {
            return Ok();
        }

        // Tambien puedes combinar attributos para restringir con multiples condiciones
        // Este Action solo puede ser llamado por Request que vengan con el HttpMethod Get o Post
        [HttpGet]
        [HttpPost]
        public IHttpActionResult GetOrPostWithHttpVerbAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Post
        [HttpPost]
        public IHttpActionResult PostWithHttpVerbAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Put
        [HttpPut]
        public IHttpActionResult PutWithHttpVerbAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Delete
        [HttpDelete]
        public IHttpActionResult DeleteWithHttpVerbAttribute()
        {
            return Ok();
        }
    }
}