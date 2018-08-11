using EjemplosFormacion.WebApi.ValueProviders;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.ValueProviderFactories
{
    // Custom Value Provider Factory que crea un Custom ValueProvider para obtener valores desde otra sources que no sea el Body o la URl (con Route Data)
    // Y posteriormente rellenar los parametros segun se requieran del Action que se va a invocar
    // Puedes llenar, cero, uno, varios o todos los parametros, ya queda de tu parte decidir
    // Como por ejemplo leer Headers y asignarle esos valores a los parametros del Action que se va a invocar
    // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
    // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
    // https://blogs.msdn.microsoft.com/jmstall/2012/04/23/how-to-create-a-custom-value-provider-in-webapi/
    public class TestHeaderValueProviderFactory : ValueProviderFactory
    {
        // Metodo que sera llamado para devolver un ValueProvider que sera el encargado de rellenar los parametros que se requieran del Action que se va a invocar
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            HttpRequestHeaders headers = actionContext.ControllerContext.Request.Headers;
            return new TestHeaderValueProvider(headers);
        }
    }
}