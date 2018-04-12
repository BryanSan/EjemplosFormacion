using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.DirectRouteProviders
{
    /// <summary>
    /// Extension del DefaultDirectRouteProvider para añadir el comportamiento de añadir una Route Template (un prefijo) antes de cualquier template definido por los RoutePrefixAttribute
    /// El Direct Route Provider will simply walk through all of the available controllers and harvest all routes declared through the use of RouteAttribute and register them. Of course, it is all not surprising – after all, this is the typical attribute routing behavior
    /// </summary>
    public class TestDirectRouteProvider : DefaultDirectRouteProvider
    {
        private readonly string _centralizedPrefix;

        public TestDirectRouteProvider(string centralizedPrefix)
        {
            _centralizedPrefix = centralizedPrefix;

            // Se quita cualquier posible "/" que pueda ver en la ruta ya que cuando se haga match se agregara
            if (!string.IsNullOrWhiteSpace(_centralizedPrefix))
            {
                _centralizedPrefix = _centralizedPrefix.Trim();
                if (_centralizedPrefix.Last() == '/')
                {
                    _centralizedPrefix = _centralizedPrefix.Remove(_centralizedPrefix.LastIndexOf("/"));
                }
            }
        }

        // Traera toda la informacion de los Controllers disponibles registrados con un RouteAttribute [Route("ruta")], 
        // Leera la configuracion y registrara la ruta para la cual haran match
        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            // Si no existe prefix configurado solo se añade el prefix configurado en este Route Provider
            if (existingPrefix == null) return _centralizedPrefix;

            // Se añade el prefix configurado en este Route Provider con el Route Prefix configurado y se devuelve
            return string.Format("{0}/{1}", _centralizedPrefix, existingPrefix);
        }
    }
}