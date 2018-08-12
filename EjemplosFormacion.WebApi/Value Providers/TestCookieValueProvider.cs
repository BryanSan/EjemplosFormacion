using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.ValueProviders
{
    // Custom Value Provider que sera el encargado de llenar de valores a los parametros del Action que se va a invocar
    // Puedes llenar, cero, uno, varios o todos los parametros, ya queda de tu parte decidir
    // Puedes llenar unos parametros con el Route Data y/o del Body y otros puedes llenarlo con este ValueProvider
    // Web Api llamara a esta clase pasandole el nombre del parametro que se quiere llenar, 
    // Segun ese nombre crearas una logica (en este caso leer una cookie con el mismo nombre del parametro a llenar) y asignara ese valor al parametro
    // Esta clase intenta llenar los parametros de un Action con un nombre con el valor de un Header con el mismo nombre
    // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
    // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
    class TestCookieValueProvider : IValueProvider
    {
        private readonly Dictionary<string, string> _cookieValues;

        public TestCookieValueProvider(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            // Llena el Diccionario de Cookies para esta Request
            _cookieValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var cookie in actionContext.Request.Headers.GetCookies())
            {
                foreach (CookieState state in cookie.Cookies)
                {
                    _cookieValues[state.Name] = state.Value;
                }
            }
        }

        public bool ContainsPrefix(string prefix)
        {
            return _cookieValues.Keys.Contains(prefix);
        }

        // Metodo que sera invocado por Web API pasandole el nombre del parametro el cual quiere obtener su valor
        // En este metodo asignaras el valor a ese parametro o devolveras null si no es de tu competencia o no puedes hacerlo
        public ValueProviderResult GetValue(string key)
        {
            string value;
            if (_cookieValues.TryGetValue(key, out value))
            {
                return new ValueProviderResult(value, value, CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}