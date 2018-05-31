using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V2
{
    public class TestVersionControllerVersusNameSpaceOfControllerController : ApiController
    {
        // Action usado para la seleccion del controller con el uso de un Custom Http Controller Selector
        // Que usara el NameSpace de este controller contra la version solicitada por el Request en diferentes maneras
        // Query, Accept Header, Media Type Header
        // TestController (NameSpace V1.TestController) y TestController (NameSpace V2.TestController) son dos tipos que seran resueltos segun la version que venga 1 o 2 y sus NameSpaces
        // Ejemplos
        // Url -> api/TestVersionControllerVersusNameSpace/TestVersion?v=1
        // Header -> X-EjemplosFormacion-Version con valor 1
        // Header -> application/json; version=1
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 2");
        }

        [Route("v{version:isSpecificValue(2)}/TestVersionControllerVersusNameSpaceOfController/TestVersionWithRouteAttribute")]
        public IHttpActionResult TestVersionWithRouteAttribute()
        {
            return Ok("Version 2");
        }
    }
}
