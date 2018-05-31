using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpRouteConstraints
{
    public class TestIntRangeHttpRouteConstraint : IHttpRouteConstraint
    {
        private readonly int _from;
        private readonly int _to;

        public TestIntRangeHttpRouteConstraint(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object valueToEvaluate;
            if (values.TryGetValue(parameterName, out valueToEvaluate) && valueToEvaluate != null)
            {
                var stringValueToEvaluate = valueToEvaluate as string;
                var intValueToEvaluate = 0;

                if (stringValueToEvaluate != null && int.TryParse(stringValueToEvaluate, out intValueToEvaluate))
                {
                    return intValueToEvaluate >= _from && intValueToEvaluate <= _to;
                }
            }

            return false;
        }
    }
}