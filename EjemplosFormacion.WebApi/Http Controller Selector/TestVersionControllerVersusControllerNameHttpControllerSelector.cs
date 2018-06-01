using EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods;
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

            // Obtenemos el valor del controller solicitado (Teniendo en cuenta si es Route convencional o Attribute Routing)
            string controllerNameOfRequest = GetControllerNameOfRequest(request);

            // Intenamos recuperar el controller segun la version solicitada
            HttpControllerDescriptor controllerDescriptorVersioned;
            if (discoveredControllers.TryGetValue(controllerNameOfRequest, out controllerDescriptorVersioned))
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


        #region Get Version From Request
        // Metodo para obtener la version en el Request de diversas maneras (Query String, Header y Custom Header) 
        // Hace default a version 1 si no encuentra ninguna en el Request
        private string GetVersion(HttpRequestMessage request)
        {
            // Obtener la version por Query String
            // Ejemplo -> api/controllerName/actionName?v=2
            string versionFromQueryString = request.GetValueFromQueryStringParameter("v");
            if (!string.IsNullOrWhiteSpace(versionFromQueryString) && versionFromQueryString != "1")
            {
                return "V" + versionFromQueryString;
            }

            // Obtener la version por Custom Header
            // Ejemplo -> X-EjemplosFormacion-Version con valor 2
            string versionFromHeader = request.GetValueFromHeader("X-EjemplosFormacion-Version");
            if (!string.IsNullOrWhiteSpace(versionFromHeader) && versionFromHeader != "1")
            {
                return "V" + versionFromHeader;
            }

            // Obtener la version por Accept Header
            // Ejemplo -> application/json; version=2
            // La "," separa los mime type el ";" define los parametros del anterior Mime Type
            string versionFromAcceptHeaderVersionParameter = request.GetParameterValueFromAcceptHeader("version");
            if (!string.IsNullOrWhiteSpace(versionFromAcceptHeaderVersionParameter) && versionFromAcceptHeaderVersionParameter != "1")
            {
                return "V" + versionFromAcceptHeaderVersionParameter;
            }

            // Obtener la version por el Route Data
            // Ejemplo -> api/v2/controllerName/actionName
            // Debe estar definido en el RouteTemplate el valor {version}
            string versionFromRoute = request.GetValueFromRouteData("version");
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
            // Si la Route no viene de Attribute Routing siempre tendra la key "controller" y mas keys segun lo definido en la Route
            // Si la Route define un {id}, entonces tambien tendra un key "id" aparte de "controller"
            // Si la Route viene de Attribute Routing entonces se debe leer el Action al que esta registrado y de ahi subir al Controller de ese Action
            IHttpRouteData routeDataOfRequest = request.GetRouteData();

            // Buscamos si la Route principal tiene la llave controller, si no la tienes que vengo de Attribute Routing
            string controllerName = (string)routeDataOfRequest.Values["controller"];

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
                // Como tenemos el nombre puro buscamos si tiene una version el Request
                string version = GetVersion(request);

                // Si tiene una version (No es version 1) se la añadimos al final
                controllerName = string.Concat(controllerName, version);
            }

            return controllerName;
        }

    }
}