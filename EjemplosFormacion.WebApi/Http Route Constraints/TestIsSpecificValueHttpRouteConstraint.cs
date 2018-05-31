using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpRouteConstraints
{
    public class TestIsSpecificValueHttpRouteConstraint : IHttpRouteConstraint
    {
        private readonly string _valueToMatch;

        public TestIsSpecificValueHttpRouteConstraint(string valueToMatch)
        {
            _valueToMatch = valueToMatch;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object valueToEvaluate;
            if (values.TryGetValue(parameterName, out valueToEvaluate) && valueToEvaluate != null)
            {
                var stringValueToEvaluate = valueToEvaluate as string;

                return _valueToMatch.Equals(stringValueToEvaluate, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}