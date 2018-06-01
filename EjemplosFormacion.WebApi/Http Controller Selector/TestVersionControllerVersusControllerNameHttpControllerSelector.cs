using EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpControllerSelector
{
    /// <summary>
    /// Custom Http Controller Selector para seleccionar un controller segun la version solicitada por el Request segun diferentes maneras
    /// Query String, Custom Header, Accept Header Parameter y Route Data
    /// Usando el nombre del controller para comparar con la version solicitada
    /// TestController y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
    /// Ejemplos
    /// Query String -> api/controllerName/actionName?v=2
    /// Custom Header -> X-EjemplosFormacion-Version con valor 2
    /// Accept Header Parameter -> application/json; version=2 ( La "," separa los mime type el ";" define los parametros del anterior Mime Type )
    /// Route Data -> api/v2/controllerName/actionName
    /// </summary>
    public class TestVersionControllerVersusControllerNameHttpControllerSelector : DefaultHttpControllerSelector
    {
        public TestVersionControllerVersusControllerNameHttpControllerSelector(HttpConfiguration config) : base(config)
        {
            
        }

        // Metodo que sera llamado para devolver el Http Controller Descriptor solicitado por la Request
        // El controller que sea devuelto es el que se usara para atender el Request
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            // Lista de todos los Controllers que la aplicacion puede usar (Todos toditos)
            IDictionary<string, HttpControllerDescriptor> discoveredControllers = GetControllerMapping();

            // Metodo para obtener la version en el Request de diversas maneras (Query String, Custom Header, Accept Header Parameter y Route Data) 
            string controllerNameOfRequest = GetControllerNameOfRequest(request);

            // Intengamos recuperar el controller segun la version solicitada
            HttpControllerDescriptor controllerDescriptorVersioned;
            if (discoveredControllers.TryGetValue(controllerNameOfRequest, out controllerDescriptorVersioned))
            {
                // Si lo tengo lo devuelvo
                return controllerDescriptorVersioned;
            }
            else
            {
                // Si no consigo ningun Controller devuelvo null y dara un error que no se consigue un Controller que atienda el Request
                return null;
            }
        }

        // Metodo para inspeccionar el RouteData y sacar el nombre del Controller teniendo en cuenta si es Route convencional o Attribute Routing
        private string GetControllerNameOfRequest(HttpRequestMessage request)
        {
            // Route Data del Request donde tiene los valores usados para hacer match 
            // Si la Route no viene de Attribute Routing siempre tendra la key "controller" y mas keys segun lo definido en la Route
            // Si la Route define un {id}, entonces tambien tendra un key "id" aparte de "controller"
            // Si la Route viene de Attribute Routing entonces se debe leer el Action al que esta registrado y de ahi subir al Controller de ese Action
            IHttpRouteData routeDataOfRequest = request.GetRouteData();

            // Buscamos si la Route tiene la llave controller
            string controllerName = (string)routeDataOfRequest.Values["controller"];

            // Si no la tienes que vengo de Attribute Routing
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                // Buscamos la SubRoute
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();

                // Buscamos el Action al que la Route esta asociada (A que Action esta asociada el Attribute Routing)
                HttpActionDescriptor[] httpActionDescriptors = (HttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];

                // Hallamos el nombre del Controller de ese Action (Ya vendra con version ya que es el nombre de la clase sin la palabra final "controller")
                // Por lo que no es necesario agregarle la version
                controllerName = httpActionDescriptors.FirstOrDefault()?.ControllerDescriptor.ControllerName;
            }
            else
            {
                // Metodo para obtener la version en el Request de diversas maneras (Query String, Custom Header, Accept Header Parameter y Route Data) 
                // Como tenemos el nombre puro (Sin Version ni Controller al final) buscamos si tiene una version el Request
                string version = GetVersion(request);

                // Si no tengo una version superior a 1 la borro ya que no existe nombre de Controller con V1 al final
                if (!string.IsNullOrWhiteSpace(version) && version != "1")
                {
                    version = "V" + version;
                }
                else
                {
                    version = null;
                }

                // Si tiene una version (No es version 1) se la añadimos al final
                controllerName = string.Concat(controllerName, version);
            }

            return controllerName;
        }

        // Metodo para obtener la version en el Request de diversas maneras (Query String, Custom Header, Accept Header Parameter y Route Data) 
        private string GetVersion(HttpRequestMessage request)
        {
            // Obtener la version por Query String
            // Ejemplo -> api/controllerName/actionName?v=2
            string versionFromQueryString = request.GetValueFromQueryStringParameter("v");
            if (!string.IsNullOrWhiteSpace(versionFromQueryString))
            {
                return versionFromQueryString;
            }

            // Obtener la version por Custom Header
            // Ejemplo -> X-EjemplosFormacion-Version con valor 2
            string versionFromHeader = request.GetValueFromHeader("X-EjemplosFormacion-Version");
            if (!string.IsNullOrWhiteSpace(versionFromHeader))
            {
                return versionFromHeader;
            }

            // Obtener la version por Accept Header
            // Ejemplo -> application/json; version=2
            // La "," separa los mime type el ";" define los parametros del anterior Mime Type
            string versionFromAcceptHeaderVersionParameter = request.GetParameterValueFromAcceptHeader("version");
            if (!string.IsNullOrWhiteSpace(versionFromAcceptHeaderVersionParameter))
            {
                return versionFromAcceptHeaderVersionParameter;
            }

            // Obtener la version por el Route Data
            // Ejemplo -> api/v2/controllerName/actionName
            // Debe estar definido en el RouteTemplate el valor {version} sea en el Attribute Routing o Route Table
            string versionFromRoute = request.GetValueFromRouteData("version");
            if (!string.IsNullOrWhiteSpace(versionFromRoute))
            {
                return versionFromRoute;
            }

            return null;
        }

    }
}