using System;
using System.Collections.Generic;
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
    /// La implementacion de esta clase define, que para el parametro en el Route que se le registre esta Http Route Constraint su valor debe ser exactamente al estipulado por esta Http Route Constraint
    /// https://docs.microsoft.com/es-es/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
    /// </summary>
    class TestIsSpecificValueHttpRouteConstraint : IHttpRouteConstraint
    {
        private readonly string _valueToMatch;

        // Constructor opcional para pasar a la clase cualquier valor que necesites, cuando registres un parametro podras pasarle valores al constructor
        // Por ejemplo Route("{id:isSpecificValue(2)}")] aqui pasamos un valor de 2 a este Constructor
        public TestIsSpecificValueHttpRouteConstraint(string valueToMatch)
        {
            _valueToMatch = valueToMatch;
        }

        // Este metodo sera llamado por Web Api para darte la oportunidad de evaluar si el valor cumple con las reglas que estableciste
        // Debes devolver true o false para denotar si pasa o no pasa
        // Puedes evaluar el Request, hallar le valor en el Dictionary de values o cualquier otra evaluacion que consideres pertinente para establecer las reglas que quieres
        // Como en este caso en el que la regla establecida especifica que el valor del parametro debe ser igual al pasado en el constructor de esta clase al momento de registrar la Route
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