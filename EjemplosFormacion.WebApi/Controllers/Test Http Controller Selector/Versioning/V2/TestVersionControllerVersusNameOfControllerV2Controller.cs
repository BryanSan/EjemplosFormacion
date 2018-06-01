﻿using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpControllerSelector.Versioning.V2
{
    public class TestVersionControllerVersusNameOfControllerV2Controller : ApiController
    {
        // Action usado para la seleccion del controller con el uso de un Custom Http Controller Selector
        // Que usara el nombre de este controller contra la version solicitada por el Request en diferentes maneras
        // Query, Accept Header, Media Type Header
        // TestController y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
        // Ejemplos
        // Url -> api/TestVersionControllerVersusNameOfControllerV2/TestVersion?v=2
        // Header -> X-EjemplosFormacion-Version con valor 2
        // Header -> application/json; version=2
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
        public IHttpActionResult TestVersion()
        {
            return Ok("Version 2");
        }

        [Route("v{version:isSpecificValue(2)}/TestVersionControllerVersusNameOfController/TestVersionWithRouteAttribute")]
        public IHttpActionResult TestVersionWithRouteAttribute()
        {
            return Ok("Version 2");
        }
    }
}
