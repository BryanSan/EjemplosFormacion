using EjemplosFormacion.WebApi.ParametersBindingAttributes.Abstract;
using EjemplosFormacion.WebApi.Stubs.Enums;

namespace EjemplosFormacion.WebApi.ParametersBindingAttributes
{
    class TestETagIfMatchAttribute : ETagMatchParameterBindingAttribute
    {
        public TestETagIfMatchAttribute() : base(TestETagMatchEnum.IfMatch)
        {
        }
    }
}