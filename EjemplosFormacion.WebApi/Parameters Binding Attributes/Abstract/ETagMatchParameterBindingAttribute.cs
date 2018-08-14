using EjemplosFormacion.WebApi.HttpParametersBindings;
using EjemplosFormacion.WebApi.Stubs.Enums;
using EjemplosFormacion.WebApi.Stubs.Models;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace EjemplosFormacion.WebApi.ParametersBindingAttributes.Abstract
{
    abstract class ETagMatchParameterBindingAttribute : ParameterBindingAttribute
    {
        private TestETagMatchEnum _match;

        public ETagMatchParameterBindingAttribute(TestETagMatchEnum match)
        {
            _match = match;
        }

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