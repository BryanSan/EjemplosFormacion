using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpRouteConstraints
{
    public class TestNonZeroHttpRouteConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object valueToEvaluate;
            if (values.TryGetValue(parameterName, out valueToEvaluate) && valueToEvaluate != null)
            {
                long longValueToEvaluate;
                if (valueToEvaluate is long)
                {
                    longValueToEvaluate = (long)valueToEvaluate;
                    return longValueToEvaluate != 0;
                }

                string stringValueToEvaluate = Convert.ToString(valueToEvaluate, CultureInfo.InvariantCulture);
                if (Int64.TryParse(stringValueToEvaluate, NumberStyles.Integer, CultureInfo.InvariantCulture, out longValueToEvaluate))
                {
                    return longValueToEvaluate != 0;
                }
            }
            return false;
        }

    }
}