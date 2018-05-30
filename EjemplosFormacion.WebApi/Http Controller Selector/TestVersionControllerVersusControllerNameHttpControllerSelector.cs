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
            // Dictionario de todos los controller disponibles en la aplicacion (Todos toditos)
            IDictionary<string, HttpControllerDescriptor> discoveredControllers = GetControllerMapping();

            // Route Data del Request donde tiene los valores usados para hacer match 
            // Siempre tendra la key "controller", tendra mas keys segun lo definido en la Route
            // Si la route define un {id}, entonces tambien tendra un key "id" aparte de "controller"
            IHttpRouteData routeDataOfRequest = request.GetRouteData();

            // Obtenemos el valor del controller solicitado
            string controllerNameOfRequest = (string)routeDataOfRequest.Values["controller"];

            // Buscamos la version
            string version = GetVersion(request);

            // Armamos el nombre del controller con la version solicitada
            string controllerNameVersioned = string.Concat(controllerNameOfRequest, "V", version);

            // Intenamos recuperar el controller segun la version solicitada
            HttpControllerDescriptor controllerDescriptorVersioned;
            if (discoveredControllers.TryGetValue(controllerNameVersioned, out controllerDescriptorVersioned))
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

        // Metodo para obtener la version en el Request de diversas maneras (Query String, Header y Custom Header) 
        // Hace default a version 1 si no encuentra ninguna en el Request
        private string GetVersion(HttpRequestMessage request)
        {
            // Obtener la version por Query String
            // Ejemplo -> api/controllerName/actionName?v=2
            string versionFromQueryString = GetVersionFromQueryString(request);
            if (!string.IsNullOrWhiteSpace(versionFromQueryString))
            {
                return versionFromQueryString;
            }

            // Obtener la version por Custom Header
            // Ejemplo -> X-EjemplosFormacion-Version con valor 2
            string versionFromHeader = GetVersionFromHeader(request);
            if (!string.IsNullOrWhiteSpace(versionFromHeader))
            {
                return versionFromHeader;
            }

            // Obtener la version por Accept Header
            // Ejemplo -> application/json; version=2
            // La "," separa los mime type el ";" define los parametros del anterior Mime Type
            string versionFromAcceptHeaderVersionParameter = GetVersionFromAcceptHeaderVersionParameter(request);
            if (!string.IsNullOrWhiteSpace(versionFromAcceptHeaderVersionParameter))
            {
                return versionFromAcceptHeaderVersionParameter;
            }

            return "1";
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
    }
}