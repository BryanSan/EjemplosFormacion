using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.HttpRouteConstraints
{
    /// <summary>
    /// Custom Http Route Constraint para registrar en una Route,
    /// Para que el parametro de la Route al que le registres esta Http Route Constraint tenga que cumplir las reglas que establece esta Http Route Constraint
    /// Si la cumple sera llamado el Action, si no, no sera ejecutada el Action y sera como si no se encontrara un Action para el Request dado
    /// Puedes usarlo para diferenciar varios Action segun sus Http Route Constraints
    /// Recordar que Web Api debe tener uno y solo un candidato de Action al final de toda la evaluacion para llamar o dara error si consigue una ambiguedad (Varios Action que pueden ejecutar el Request)
    /// La implementacion de esta clase define, que para el parametro en el Route que se le registre esta Http Route Constraint su valor debe ser diferente a 0
    /// https://docs.microsoft.com/es-es/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
    /// </summary>
    class TestNonZeroHttpRouteConstraint : IHttpRouteConstraint
    {
        // Este metodo sera llamado por Web Api para darte la oportunidad de evaluar si el valor cumple con las reglas que estableciste
        // Debes devolver true o false para denotar si pasa o no pasa
        // Puedes evaluar el Request, hallar le valor en el Dictionary de values o cualquier otra evaluacion que consideres pertinente para establecer las reglas que quieres
        // Como en este caso en el que la regla establecida especifica que el valor del parametro debe ser diferente a 0
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