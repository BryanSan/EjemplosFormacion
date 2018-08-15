using EjemplosFormacion.WebApi.HttpParametersBindings;
using EjemplosFormacion.WebApi.Stubs.Enums;
using EjemplosFormacion.WebApi.Stubs.Models;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace EjemplosFormacion.WebApi.ParametersBindingAttributes.Abstract
{
    /// <summary>
    /// Custom Parameter Binding Attribute que sirve de Factory para devolver un HttpParameterBinding
    /// Adorna un parametro de un Action con este Attribute para que Web Api sepa que debe bindear ese parametro con el HttpParameterBinding devuelto por este Attribute
    /// En este caso el HttpParameterBinding que estamos devolviendo bindeara el parametro que estamos adornando con el valor del header If-Match o If-NoneMatch
    /// </summary>
    abstract class ETagMatchParameterBindingAttribute : ParameterBindingAttribute
    {
        private TestETagMatchEnum _match;

        public ETagMatchParameterBindingAttribute(TestETagMatchEnum match)
        {
            _match = match;
        }

        // Metodo que sera llamado por Web Api para que devolvamos la instancia de HttpParameterBinding a usar
        // Aqui estamos evaluando que el tipo sea correcto y devolvemos el HttpParameterBinding que deseeamos usar
        // Si no es el Type devolvemos un Binding Error
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(TestETagModel))
            {
                return new TestETagHttpParameterBinding(parameter, _match);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}