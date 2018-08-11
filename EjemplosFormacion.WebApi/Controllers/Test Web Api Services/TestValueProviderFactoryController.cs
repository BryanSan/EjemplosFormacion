using EjemplosFormacion.WebApi.ValueProviderFactories;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.Controllers.TestWebApiServices
{
    public class TestValueProviderFactoryController : ApiController
    {
        // Metodo que usa un Custom Value Provider para llenar el userAgent y host con los valores de Header que tengan el mismo nombre
        // El id vendra del RouteData o QueryString 
        // De esta manera puedes llenar valores del parametro del Action con otras sources que consideres pertinente, incluso haciendo una combinacion de ellas
        // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
        // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
        // Los Value Providers que seran usados para obtener los valores son los configurados para el Servicio y/o Controller
        public object TestValueProviderFactory(int id, [ModelBinder] string userAgent, [ModelBinder] string host)
        {
            return string.Format(@"User agent: {0}, host: {1}, id: {2}", userAgent, host, id);
        }

        // Metodo que usa un Custom Value Provider para llenar el userAgent con los valores de Header que tengan el mismo nombre
        // De esta manera puedes llenar valores del parametro del Action con otras sources que consideres pertinente, incluso haciendo una combinacion de ellas
        // En este caso a diferencia del anterior, estas especificando que Value Provider Factory sera usado para obtener el Value Provider que sera el encargado de obtener el valor para este parametro
        // En el anterior se usan los configurados para el Servicio y/o Controller, aqui lo especificamos InLine
        public object TestValueProviderFactoryWithAttribute([ValueProvider(typeof(TestHeaderValueProviderFactory))] string userAgent)
        {
            return string.Format(@"User agent: {0}", userAgent);
        }
    }
}