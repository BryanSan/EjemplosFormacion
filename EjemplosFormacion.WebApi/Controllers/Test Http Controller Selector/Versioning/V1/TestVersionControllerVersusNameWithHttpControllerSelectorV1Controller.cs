using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V1
{
    public class TestVersionControllerVersusNameWithHttpControllerSelectorV1Controller : ApiController
    {
        // Action usado para la seleccion del controller con el uso de un Custom Http Controller Selector
        // Que usara el nombre de este controller contra la version solicitada por el Request en diferentes maneras
        // Query, Accept Header, Media Type Header
        // TestV1Controller y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
        // Ejemplos
        // Url -> api/TestVersionControllerVersusNameWithHttpControllerSelectorV1/TestVersion?v=1
        // Header -> X-EjemplosFormacion-Version con valor 1
        // Header -> application/json; version=1
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 1");
        }
    }
}
