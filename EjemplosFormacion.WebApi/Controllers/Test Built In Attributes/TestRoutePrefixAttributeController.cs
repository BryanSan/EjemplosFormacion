using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestBuiltInAttributes
{
    // Esta ruta define un nuevo template usado para que el Web Api haga match al Controller
    // Normalmente se llamaria a este Controller y a su Action como TestRoutePrefixAttribute/TestRoutePrefixAttribute (Controller + Action)
    // Pero como hemos definido un RoutePrefix el default template del Controller se cambia por el string que hallas configurado en el attributo("RoutePrefix" en este caso)
    // Quedando la ruta como RoutePrefix/TestRoutePrefixAttribute (RoutePrefixString + Action)
    // Puedes crear un RoutePrefix global (para que todos los RoutePrefix tengan algo antes o despues de lo que se configure) 
    // Configurando y registrando un Direct Route Provider (Que ya esta configurado para este proyecto que agrega "api" al route prefix) 
    // Y asignarle el valor de la ruta que quieres que se agregue a todas las rutas configuradas en todos los attributos RoutePrefix
    // RoutePrefix debe ser usado en conjuncion con el atributo [Route] para definir una ruta completa
    // Si usas un RoutePrefix solo te dara error y no hallara el Action
    // Quedando la ruta (RoutePrefix + Route) con posibilidad de tener algo o despues si hay un Direct Route Provider como es en este caso que tiene un "api" configurado
    // Para llegar a este Action usar api/RoutePrefix/TestRoutePrefixAttribute
    // (DirectRouteProviderPrefix + RoutePrefix + Route)
    [RoutePrefix("RouteAndRoutePrefix")]
    public class TestRouteAndRoutePrefixAttributeController : ApiController
    {
        // Para llegar a este Action usar api/RoutePrefix/TestRoutePrefixAttribute
        // (DirectRouteProviderPrefix + RoutePrefix + Route)
        [Route(nameof(TestRouteAttribute))]
        public IHttpActionResult TestRouteAttribute()
        {
            return Ok();
        }
    }
}
