using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Routing;

namespace EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Extension Method para hallar el valor de un parametro del Query String segun el nombre especificado
        /// Ejemplo -> api/controllerName/actionName?v=2
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryStringParameterName"></param>
        /// <returns></returns>
        public static string GetValueFromQueryStringParameter(this HttpRequestMessage request, string queryStringParameterName)
        {
            // Parseamos el Query String
            NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            // Buscamos en el Query String el valor de la version solicitada, en este caso el valor de "v"
            // Tambien puedes cambiarlo por "version" o cualquier nombre que quieras, pero que sea el definido por la aplicacion
            string version = query[queryStringParameterName];

            return version;
        }

        /// <summary>
        /// Extension Method para hallar el valor de un Header segun el nombre especificado
        /// Ejemplo -> X-EjemplosFormacion-Version con valor 2
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static string GetValueFromHeader(this HttpRequestMessage request, string headerName)
        {
            return GetValuesFromHeader(request, headerName)?.FirstOrDefault();
        }

        /// <summary>
        /// Extension Method para hallar los valores de un Header segun el nombre especificado
        /// Ejemplo -> X-EjemplosFormacion-Version con valor 2
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetValuesFromHeader(this HttpRequestMessage request, string headerName)
        {
            // Buscamos si esta el Custom Header en el Request
            if (request.Headers.Contains(headerName))
            {
                // Si lo esta recuperamos el Valor y lo devolvemos
                IEnumerable<string> headerValues = request.Headers.GetValues(headerName);
                return headerValues;
            }

            return null;
        }

        /// <summary>
        /// Extension Method para hallar el valor de un parametro del Accept Header segun el nombre especificado
        /// Recordar que la "," separa los mime type y el ";" define los parametros del anterior Mime Type
        /// Ejemplo -> application/json; version=2
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static string GetParameterValueFromAcceptHeader(this HttpRequestMessage request, string parameterName)
        {
            return GetParameterValuesFromAcceptHeader(request, parameterName)?.FirstOrDefault();
        }

        /// <summary>
        /// Extension Method para hallar los valores de un parametro del Accept Header segun el nombre especificado
        /// Recordar que la "," separa los mime type y el ";" define los parametros del anterior Mime Type
        /// Ejemplo -> application/json; version=2
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetParameterValuesFromAcceptHeader(this HttpRequestMessage request, string parameterName)
        {
            // Buscamos el Accept Header
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeader = request.Headers.Accept;

            // Iteramos en todos los Mime Type que tenga el Accept Header
            foreach (MediaTypeWithQualityHeaderValue mimeTypeInAcceptHeader in acceptHeader)
            {
                // Consultamos si el Mime Type tiene el parametro
                if (mimeTypeInAcceptHeader.Parameters.Any(x => x.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase)))
                {
                    // Si lo tiene entonces obtenemos su valor y lo devolvemos
                    IEnumerable<string> mimeParameterVersion = mimeTypeInAcceptHeader.Parameters
                                                                       .Where(x => x.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                                                                       .Select(x => x.Value);

                    return mimeParameterVersion;
                }
            }

            return null;
        }

        /// <summary>
        /// Extension Method para hallar el valor otorgado por el Web API al Route Data segun el nombre especificado
        /// Ejemplo ->
        /// Si especificas que quieres el route data "version" y tienes una ruta definida como api/{version}/controllerName/actionName
        /// Lo que el Web API le asigne al {version}, sera el valor que recuperaras
        /// Debe estar definido en el RouteTemplate el valor {version} para poder recuperar su valor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeDataName"></param>
        /// <returns></returns>
        public static string GetValueFromRouteData(this HttpRequestMessage request, string routeDataName)
        {
            IHttpRouteData routeDataOfRequest = request.GetRouteData();
            string versionFromRoute = routeDataOfRequest.Values[routeDataName] as string;

            if (string.IsNullOrWhiteSpace(versionFromRoute))
            {
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();
                if (subRouteData != null && subRouteData.Values.ContainsKey(routeDataName))
                {
                    string versionFromSubRoute = subRouteData.Values[routeDataName] as string;
                    return versionFromSubRoute;
                }
            }

            return versionFromRoute;
        }
    }
}
