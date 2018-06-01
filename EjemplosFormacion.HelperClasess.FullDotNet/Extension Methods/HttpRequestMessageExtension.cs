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
    public static class HttpRequestMessageExtension
    {
        // Ejemplo -> api/controllerName/actionName?v=2
        public static string GetValueFromQueryStringParameter(this HttpRequestMessage request, string queryStringParameterName)
        {
            // Parseamos el Query String
            NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            // Buscamos en el Query String el valor de la version solicitada, en este caso el valor de "v"
            // Tambien puedes cambiarlo por "version" o cualquier nombre que quieras, pero que sea el definido por la aplicacion
            string version = query[queryStringParameterName];

            return version;
        }

        public static string GetValueFromHeader(this HttpRequestMessage request, string headerName)
        {
            return GetValuesFromHeader(request, headerName)?.FirstOrDefault();
        }

        // Ejemplo -> X-EjemplosFormacion-Version con valor 2
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

        public static string GetParameterValueFromAcceptHeader(this HttpRequestMessage request, string parameterName)
        {
            return GetParameterValuesFromAcceptHeader(request, parameterName)?.FirstOrDefault();
        }

        // Ejemplo -> application/json; version=2
        // La "," separa los mime type el ";" define los parametros del anterior Mime Type
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

        // Obtener la version por el Route Data
        // Ejemplo -> api/v2/controllerName/actionName
        // Debe estar definido en el RouteTemplate el valor {version}
        public static string GetValueFromRouteData(this HttpRequestMessage request, string routeDataName)
        {
            IHttpRouteData routeDataOfRequest = request.GetRouteData();
            string versionFromRoute = routeDataOfRequest.Values[routeDataName] as string;

            if (string.IsNullOrWhiteSpace(versionFromRoute))
            {
                IHttpRouteData subRouteData = routeDataOfRequest.GetSubRoutes()?.FirstOrDefault();
                if (subRouteData != null)
                {
                    string versionFromSubRoute = subRouteData.Values[routeDataName] as string;
                    return versionFromSubRoute;
                }
            }

            return versionFromRoute;
        }
    }
}
