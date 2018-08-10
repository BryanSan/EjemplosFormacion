using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestBuiltInAttributes
{
    public class TestHttpVerbAttributeController : ApiController
    {
        // Tambien puedes combinar attributos para restringir con multiples condiciones
        // Este Action solo puede ser llamado por Request que vengan con el HttpMethod Get o Post
        [HttpGet]
        [HttpPost]
        public IHttpActionResult TestMultipleHttpVerbGetPostAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Get
        [HttpGet]
        public IHttpActionResult TestHttpGetAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Post
        [HttpPost]
        public IHttpActionResult TestHttpPostAttribute()
        {
            return Ok();
        }
        
        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Put
        [HttpPut]
        public IHttpActionResult TestHttpPutAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Delete
        [HttpDelete]
        public IHttpActionResult TestHttpDeleteAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Head
        [HttpHead]
        public IHttpActionResult TestHttpHeadAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Options
        [HttpOptions]
        public IHttpActionResult TestHttpOptionsAttribute()
        {
            return Ok();
        }

        // Restringe este Action a solo ser llamado por Request que vengan con el HttpMethod Patch
        [HttpPatch]
        public IHttpActionResult TestHttpPatchAttribute()
        {
            return Ok();
        }
    }
}