using EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods;
using System;
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
    /// Usando el NameSpace del controller para comparar con la version solicitada
    /// TestController (NameSpace V1.TestController) y TestController (NameSpace V2.TestController) son dos tipos que seran resueltos segun la version que venga 1 o 2 y sus NameSpaces
    /// Ejemplos
    /// Query String -> api/controllerName/actionName?v=2
    /// Custom Header -> X-EjemplosFormacion-Version con valor 2
    /// Accept Header Parameter -> application/json; version=2 ( La "," separa los mime type el ";" define los parametros del anterior Mime Type )
    /// Route Data -> api/v2/controllerName/actionName
    /// </summary>
    public class TestVersionControllerVersusControllerNameSpaceHttpControllerSelector : IHttpControllerSelector
    {
        // Diccionario de todos los Controllers en la aplicacion, seran usados para mapear las Request luego
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _discoveredControllers;
        private readonly HttpConfiguration _config;

        public TestVersionControllerVersusControllerNameSpaceHttpControllerSelector(HttpConfiguration config)
        {
            _config = config;
            _discoveredControllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }


        #region Metodos Contrato
        // Dictionario de todos los controller disponibles en la aplicacion (Todos toditos)
        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _discoveredControllers.Value;
        }

        // Metodo que sera llamado para devolver el Http Controller Descriptor solicitado por la Request
        // El controller que sea devuelto es el que se usara para atender el Request
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            // Route Data del Request donde tiene los valores usados para hacer match 
            // Siempre tendra la key "controller", tendra mas keys segun lo definido en la Route
            // Si la route define un {id}, entonces tambien tendra un key "id" aparte de "controller"
            IHttpRouteData routeDataOfRequest = request.GetRouteData();

            // Metodo para inspeccionar el RouteData y sacar el nombre del Controller teniendo en cuenta si es Route convencional o Attribute Routing
            string controllerNameOfRequest = GetControllerNameOfRequest(routeDataOfRequest);

            // Metodo para obtener la version en el Request de diversas maneras (Query String, Custom Header, Accept Header Parameter y Route Data) 
            string version = GetVersion(request);

            // Armamos el nombre del controller con la version solicitada, si no tiene un numero de version lo dejamos como esta
            // Esto para no molestar los otros Controllers de la Solucion de Formacion que no tienen version
            string controllerNameToMatch = controllerNameOfRequest;
            if (!string.IsNullOrWhiteSpace(version))
            {
                controllerNameToMatch = string.Format("v{0}.{1}", version, controllerNameOfRequest);
            }

            // Intenamos recuperar el controller segun la version solicitada
            HttpControllerDescriptor controllerDescriptorVersioned;
            if (_discoveredControllers.Value.TryGetValue(controllerNameToMatch, out controllerDescriptorVersioned))
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
        #endregion


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

        // Metodo para inspeccionar el RouteData y sacar el nombre del Controller teniendo en cuenta si es Route convencional o Attribute Routing
        private static string GetControllerNameOfRequest(IHttpRouteData routeDataOfRequest)
        {
            // Buscamos si la Route tiene la llave controller
            string controllerName = (string)routeDataOfRequest.Values["controller"];

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                // Buscamos la SubRoute
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();

                // Buscamos el Action al que la Route esta asociada (A que Action esta asociada el Attribute Routing)
                HttpActionDescriptor[] httpActionDescriptors = (HttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];

                // Hallamos el nombre del Controller de ese Action 
                controllerName = httpActionDescriptors.FirstOrDefault()?.ControllerDescriptor.ControllerType.Name;
            }
            else
            {
                // Añadimos el Controller al final para que haga match con el Dictionary de posibles Controllers
                controllerName += "Controller";
            }

            return controllerName;
        }

        // Metodo que recupera todos los Controllers posibles en el Web Api (Estos Controllers seran mapeados luego por las Request)
        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            // Obtenemos el resolver del Assembly
            IAssembliesResolver assembliesResolver = _config.Services.GetAssembliesResolver();

            // Obtenemos el resolver de los Controllers
            IHttpControllerTypeResolver controllersResolver = _config.Services.GetHttpControllerTypeResolver();

            // Resolvemos todos los Controllers del Assembly
            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            // Recorremos cada Controller y los registramos en el Dictionary para su posterior mapeo con las Request
            // Puedes armar el Dictionary como quieras, queda de tu parte ver como lo trabajas aqui y luego con las Request
            // Eso si si no esta aqui entonces no se mapeara a las Request
            foreach (Type controllerType in controllerTypes)
            {
                // Obtenemos todo el Full Name del Controller y separamos los segmentos (.)
                string[] fullNameSplitted = controllerType.FullName.ToLowerInvariant().Split('.');

                // El nombre del controller es el ultimo por obligacion
                string controllerName = fullNameSplitted[fullNameSplitted.Length - 1];

                // El numero de version es el penultimo por convencion
                string versionController = fullNameSplitted[fullNameSplitted.Length - 2];

                // Creamos una unique key para el Controller
                // Esto para no molestar los otros Controllers de la Solucion de Formacion que no tienen version
                string key = controllerName;
                if (versionController.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                {
                    key = versionController + "." + key;
                }

                // Registramos el Controller con la unique key
                dictionary[key] = new HttpControllerDescriptor(_config, controllerType.Name, controllerType);
            }

            return dictionary;
        }
    }
}