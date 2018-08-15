using EjemplosFormacion.WebApi.DirectRouteFactories;
using EjemplosFormacion.WebApi.DirectRouteProviders;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ExtensionMethods
{
    /// <summary>
    /// Extension Method para la clase HttpConfiguration para registrar una ruta de manera Tipada un template para una ruta
    /// Se pueden usar Constraints y demas
    /// Por debajo crea un IDirectRouteFactory para la configuracion dada y lo registra en el Direct Route Provider para que registra la ruta configurada
    /// </summary>
    static class HttpConfigurationExtensions
    {
        public static TestTypedDirectRouteFactory RegisterTypedRoute(this HttpConfiguration config, string template, Action<TestTypedDirectRouteFactory> configSetup)
        {
            if (string.IsNullOrWhiteSpace(template)) throw new ArgumentException("template vacio!.");
            if (configSetup == null) throw new ArgumentException("configSetup vacio!.");

            // Creamos el IDirectRouteFactory en este caso TestTypedDirectRouteFactory para registrar la ruta en el Direct Route Provider
            var route = new TestTypedDirectRouteFactory(template);
            configSetup(route);

            // Buscamos el Key (Controller Type) si ya esta buscamos el diccionario de rutas de este Controller Type y añadimos la nueva ruta
            if (TestGlobalPrefixDirectRouteProvider.routesDictionary.ContainsKey(route.ControllerType))
            {
                Dictionary<string, TestTypedDirectRouteFactory> controllerLevelDictionary = TestGlobalPrefixDirectRouteProvider.routesDictionary[route.ControllerType];
                controllerLevelDictionary.Add(route.ActionName, route);
            }
            else
            {
                // Si no esta creamos una entrada para ese Controller Type y otro diccionario para sus rutas 
                // Y lo añadimos al Diccionario estatico del Direct Route Provider para que registre las rutas configuradas
                var controllerLevelDictionary = new Dictionary<string, TestTypedDirectRouteFactory> { { route.ActionName, route } };
                TestGlobalPrefixDirectRouteProvider.routesDictionary.TryAdd(route.ControllerType, controllerLevelDictionary);
            }

            return route;
        }

    }
}