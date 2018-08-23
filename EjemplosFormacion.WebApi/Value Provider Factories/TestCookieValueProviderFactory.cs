using EjemplosFormacion.WebApi.ValueProviders;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.ValueProviderFactories
{
    // Custom Value Provider Factory que crea un Custom ValueProvider para obtener valores desde otra sources que no sea el Body o la URl (con Route Data)
    // Y posteriormente rellenar los parametros segun se requieran del Action que se va a invocar
    // Puedes llenar, cero, uno, varios o todos los parametros, ya queda de tu parte decidir
    // Como por ejemplo leer las Cookies y asignarle esos valores a los parametros del Action que se va a invocar
    // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
    // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
    public class TestCookieValueProviderFactory : ValueProviderFactory
    {
        // Metodo que sera llamado para devolver un ValueProvider que sera el encargado de rellenar los parametros que se requieran del Action que se va a invocar
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new TestCookieValueProvider(actionContext);
        }
    }
}