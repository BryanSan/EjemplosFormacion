using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V1
{
    public class TestVersionControllerVersusNameSpaceOfControllerController : ApiController
    {
        // Action usado para la seleccion del controller con el uso de un Custom Http Controller Selector
        // Que usara el NameSpace de este controller contra la version solicitada por el Request en diferentes maneras
        // Query String, Custom Header, Accept Header Parameter y Route Data
        // TestController (NameSpace V1.TestController) y TestController (NameSpace V2.TestController) son dos tipos que seran resueltos segun la version que venga 1 o 2 y sus NameSpaces
        // Ejemplos
        // Query String -> api/controllerName/actionName?v=2
        // Custom Header -> X-EjemplosFormacion-Version con valor 2
        // Accept Header Parameter -> application/json; version=2 ( La "," separa los mime type el ";" define los parametros del anterior Mime Type )
        // Route Data -> api/v2/controllerName/actionName
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 1");
        }

        [Route("v{version:isSpecificValue(1)}/TestVersionControllerVersusNameSpaceOfController/TestVersionWithRouteAttribute")]
        public IHttpActionResult TestVersionWithRouteAttribute()
        {
            return Ok("Version 1");
        }
    }
}
