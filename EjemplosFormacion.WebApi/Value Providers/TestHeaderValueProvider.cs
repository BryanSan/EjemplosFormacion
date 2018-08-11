using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.ValueProviders
{
    // Custom Value Provider que sera el encargado de llenar de valores a los parametros del Action que se va a invocar
    // Puedes llenar, cero, uno, varios o todos los parametros, ya queda de tu parte decidir
    // Puedes llenar unos parametros con el Route Data y/o del Body y otros puedes llenarlo con este ValueProvider
    // Web Api llamara a esta clase pasandole el nombre del parametro que se quiere llenar, 
    // Segun ese nombre crearas una logica (en este caso leer un header con el mismo nombre del parametro a llenar) y asignara ese valor al parametro
    // Esta clase intenta llenar los parametros de un Action con un nombre con el valor de un Header con el mismo nombre
    // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
    // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
    // https://blogs.msdn.microsoft.com/jmstall/2012/04/23/how-to-create-a-custom-value-provider-in-webapi/
    class TestHeaderValueProvider : IValueProvider
    {
        readonly HttpRequestHeaders _requestHeaders;

        public TestHeaderValueProvider(HttpRequestHeaders requestHeaders)
        {
            _requestHeaders = requestHeaders;
        }

        public bool ContainsPrefix(string prefix)
        {
            PropertyInfo headerPropertyInfo = GetHeaderPropertyInfo(prefix);
            return headerPropertyInfo != null;
        }
        
        // Metodo que sera invocado por Web API pasandole el nombre del parametro el cual quiere obtener su valor
        // En este metodo asignaras el valor a ese parametro o devolveras null si no es de tu competencia o no puedes hacerlo
        public ValueProviderResult GetValue(string actionParameterName)
        {
            PropertyInfo headerPropertyInfo = GetHeaderPropertyInfo(actionParameterName);
            if (headerPropertyInfo != null)
            {
                string headerValue = headerPropertyInfo.GetValue(_requestHeaders, null).ToString();
                return new ValueProviderResult(headerValue, headerValue, CultureInfo.InvariantCulture);
            }
            return null; // none
        }

        // Headers doesn't support property bag lookup interface, so grab it with reflection.
        PropertyInfo GetHeaderPropertyInfo(string headerName)
        {
            PropertyInfo headerPropertyInfo = typeof(HttpRequestHeaders).GetProperty(headerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            return headerPropertyInfo;
        }
    }
}