using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V1
{
    public class TestVersionControllerVersusNameOfControllerController : ApiController
    {
        // Action usado para la seleccion del controller con el uso de un Custom Http Controller Selector
        // Que usara el nombre de este controller contra la version solicitada por el Request en diferentes maneras
        // Query, Accept Header, Media Type Header
        // TestController y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
        // Ejemplos
        // Url -> api/TestVersionControllerVersusNameOfControllerV1/TestVersion?v=1
        // Header -> X-EjemplosFormacion-Version con valor 1
        // Header -> application/json; version=1
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 1");
        }

        [Route("v{version:isSpecificValue(1)}/TestVersionControllerVersusNameOfController/TestVersionWithRouteAttribute")]
        public IHttpActionResult TestVersionWithRouteAttribute()
        {
            return Ok("Version 1");
        }
    }
}
