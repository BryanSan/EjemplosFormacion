using EjemplosFormacion.WebApi.DirectRouteFactories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.DirectRouteProviders
{
    /// <summary>
    /// El DefaultDirectRouteProvider se usa para registrar las Routes que se han configurado directamente tanto en un Action como en un Controller, 
    /// Puedes modificar su comportamiento para que use Routes a tu gusto
    /// Extension del DefaultDirectRouteProvider para añadir el comportamiento de añadir una Route Template (un prefijo) antes de cualquier template definido por los RoutePrefixAttribute
    /// Tambien tiene el comportamiento de dar soporte a rutas tipadas mediante un diccionario estatico que se va llenando con la ayuda de Extension Methods del HttpConfiguration para posteriormente registrar estas rutas
    /// El Direct Route Provider will simply walk through all of the available controllers and harvest all routes declared through the use of RouteAttribute and register them. Of course, it is all not surprising – after all, this is the typical attribute routing behavior
    /// </summary>
    class TestDirectRouteProvider : DefaultDirectRouteProvider
    {
        internal static readonly ConcurrentDictionary<Type, Dictionary<string, TestTypedDirectRouteFactory>> Routes = new ConcurrentDictionary<Type, Dictionary<string, TestTypedDirectRouteFactory>>();
        private readonly string _centralizedPrefix;

        public TestDirectRouteProvider(string centralizedPrefix)
        {
            if (string.IsNullOrWhiteSpace(centralizedPrefix)) throw new ArgumentException("centralizedPrefix vacio!.");

            _centralizedPrefix = centralizedPrefix;

            // Se quita cualquier posible "/" que pueda ver en la ruta ya que cuando se haga match se agregara
            _centralizedPrefix = _centralizedPrefix.Trim();
            if (_centralizedPrefix.Last() == '/')
            {
                _centralizedPrefix = _centralizedPrefix.Remove(_centralizedPrefix.LastIndexOf("/"));
            }
        }

        // Traera toda la informacion de los Controllers disponibles registrados con un RouteAttribute o RoutePrefix [Route("ruta")], 
        // Leera la configuracion y registrara la ruta para la cual haran match
        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            string existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            // Si no existe prefix configurado solo se añade el prefix configurado en este Route Provider
            if (existingPrefix == null) return _centralizedPrefix;

            // Se añade el prefix configurado en este Route Provider con el Route Prefix configurado y se devuelve
            return string.Format("{0}/{1}", _centralizedPrefix, existingPrefix);
        }

        // Traera toda la informacion de los Controller disponibles registrados con un IDirectRouteFactory como lo es [Route]
        // En este caso lo modificamos para que añadiera las Routes Tipadas que se configuraron en el Dictionary
        // Estas Direct Route Factory seran usadas para crear las Routes que haran match con el Action
        // Recordar que las Direct Route Factory [Route] se puede poner solo en Action
        // Puede llamarse igual para Action que no tengan ningun Direct Route Factory, en ese caso la variable factories tendra Count 0
        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            List<IDirectRouteFactory> factories = base.GetActionRouteFactories(actionDescriptor).ToList();

            if (Routes.ContainsKey(actionDescriptor.ControllerDescriptor.ControllerType))
            {
                Dictionary<string, TestTypedDirectRouteFactory> controllerLevelDictionary = Routes[actionDescriptor.ControllerDescriptor.ControllerType];
                if (controllerLevelDictionary.ContainsKey(actionDescriptor.ActionName))
                {
                    factories.Add(controllerLevelDictionary[actionDescriptor.ActionName]);
                }
            }

            return factories;
        }
    }
}