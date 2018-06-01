using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpControllerSelector
{
    /// <summary>
    /// Custom Http Controller Selector para seleccionar un controller segun la version solicitada por el Request segun diferentes maneras
    /// Query, Accept Header, Media Type Header
    /// Usando el nombre del controller para comparar con la version solicitada
    /// TestV1Controller y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
    /// Ejemplos
    /// Url -> api/controllerName/actionName?v=2
    /// Header -> X-EjemplosFormacion-Version con valor 2
    /// Header -> application/json; version=2
    /// La "," separa los mime type el ";" define los parametros del anterior Mime Type
    /// </summary>
    public class TestVersionControllerVersusControllerNameHttpControllerSelector : IHttpControllerSelector
    {
        // Diccionario de todos los Controllers en la aplicacion, seran usados para mapear las Request luego
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _discoveredControllers;
        private readonly HttpConfiguration _config;

        public TestVersionControllerVersusControllerNameHttpControllerSelector(HttpConfiguration config)
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
            // Obtenemos el valor del controller solicitado
            string controllerNameOfRequest = GetControllerNameOfRequest(request);

            // Armamos el nombre del controller con la version solicitada
            string controllerNameVersioned = string.Concat(controllerNameOfRequest);

            // Intenamos recuperar el controller segun la version solicitada
            HttpControllerDescriptor controllerDescriptorVersioned;
            if (_discoveredControllers.Value.TryGetValue(controllerNameVersioned, out controllerDescriptorVersioned))
            {
                // Si lo tengo lo devuelvo
                return controllerDescriptorVersioned;
            }
            else
            {
                // Si no consigo ningun Controller ni siquiera el base, devuelvo null y dara un error que no se consigue un Controller que atienda el Request
                return null;
            }
        }
        #endregion


        #region Get Version From Request
        // Metodo para obtener la version en el Request de diversas maneras (Query String, Header y Custom Header) 
        // Hace default a version 1 si no encuentra ninguna en el Request
        private string GetVersion(HttpRequestMessage request)
        {
            // Obtener la version por Query String
            // Ejemplo -> api/controllerName/actionName?v=2
            string versionFromQueryString = GetVersionFromQueryString(request);
            if (!string.IsNullOrWhiteSpace(versionFromQueryString) && versionFromQueryString != "1")
            {
                return "V" + versionFromQueryString;
            }

            // Obtener la version por Custom Header
            // Ejemplo -> X-EjemplosFormacion-Version con valor 2
            string versionFromHeader = GetVersionFromHeader(request);
            if (!string.IsNullOrWhiteSpace(versionFromHeader) && versionFromHeader != "1")
            {
                return "V" + versionFromHeader;
            }

            // Obtener la version por Accept Header
            // Ejemplo -> application/json; version=2
            // La "," separa los mime type el ";" define los parametros del anterior Mime Type
            string versionFromAcceptHeaderVersionParameter = GetVersionFromAcceptHeaderVersionParameter(request);
            if (!string.IsNullOrWhiteSpace(versionFromAcceptHeaderVersionParameter) && versionFromAcceptHeaderVersionParameter != "1")
            {
                return "V" + versionFromAcceptHeaderVersionParameter;
            }

            // Obtener la version por el Route Data
            // Ejemplo -> api/v2/controllerName/actionName
            // Debe estar definido en el RouteTemplate el valor {version}
            string versionFromRoute = GetVersionFromRouteData(request);
            if (!string.IsNullOrWhiteSpace(versionFromRoute) && versionFromRoute != "1")
            {
                return "V" + versionFromRoute;
            }

            return null;
        }

        // Ejemplo -> api/controllerName/actionName?v=2
        private static string GetVersionFromQueryString(HttpRequestMessage request)
        {
            // Parseamos el Query String
            NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            // Buscamos en el Query String el valor de la version solicitada, en este caso el valor de "v"
            // Tambien puedes cambiarlo por "version" o cualquier nombre que quieras, pero que sea el definido por la aplicacion
            string version = query["v"];

            return version;
        }

        // Ejemplo -> X-EjemplosFormacion-Version con valor 2
        private static string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HeaderName = "X-EjemplosFormacion-Version";

            // Buscamos si esta el Custom Header en el Request
            if (request.Headers.Contains(HeaderName))
            {
                // Si lo esta recuperamos el Valor y lo devolvemos
                string headerValue = request.Headers.GetValues(HeaderName).FirstOrDefault();
                return headerValue;
            }

            return null;
        }

        // Ejemplo -> application/json; version=2
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
        private static string GetVersionFromAcceptHeaderVersionParameter(HttpRequestMessage request)
        {
            // Buscamos el Accept Header
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeader = request.Headers.Accept;

            // Iteramos en todos los Mime Type que tenga el Accept Header
            foreach (MediaTypeWithQualityHeaderValue mimeTypeInAcceptHeader in acceptHeader)
            {
                // Consultamos si el Mime Type tiene el parametro version
                if (mimeTypeInAcceptHeader.Parameters.Any(x => x.Name.Equals("version", StringComparison.OrdinalIgnoreCase)))
                {
                    // Si lo tiene entonces obtenemos su valor y lo devolvemos
                    NameValueHeaderValue mimeParameterVersion = mimeTypeInAcceptHeader.Parameters
                                                                       .Where(x => x.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                                                                       .FirstOrDefault();

                    return mimeParameterVersion.Value;
                }
            }

            return null;
        }

        // Obtener la version por el Route Data
        // Ejemplo -> api/v2/controllerName/actionName
        // Debe estar definido en el RouteTemplate el valor {version}
        public static string GetVersionFromRouteData(HttpRequestMessage request)
        {
            IHttpRouteData routeDataOfRequest = request.GetRouteData();
            string versionFromRoute = routeDataOfRequest.Values["version"] as string;

            if (string.IsNullOrWhiteSpace(versionFromRoute))
            {
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();
                if (subRouteData != null)
                {
                    string versionFromSubRoute = subRouteData.Values["version"] as string;
                    return versionFromSubRoute;
                }
            }

            return versionFromRoute;
        }
        #endregion


        private string GetControllerNameOfRequest(HttpRequestMessage request)
        {
            // Route Data del Request donde tiene los valores usados para hacer match 
            // Siempre tendra la key "controller", tendra mas keys segun lo definido en la Route
            // Si la route define un {id}, entonces tambien tendra un key "id" aparte de "controller"
            IHttpRouteData routeDataOfRequest = request.GetRouteData();

            string controllerName = (string)routeDataOfRequest.Values["controller"];
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();
                HttpActionDescriptor[] httpActionDescriptors = (HttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];
                controllerName = httpActionDescriptors.FirstOrDefault()?.ControllerDescriptor.ControllerType.Name;
            }
            else
            {
                string version = GetVersion(request);
                controllerName = string.Concat(controllerName, version, "controller");
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
                // Registramos el Controller con la unique key
                dictionary[controllerType.Name] = new HttpControllerDescriptor(_config, controllerType.Name, controllerType);
            }

            return dictionary;
        }
    }
}