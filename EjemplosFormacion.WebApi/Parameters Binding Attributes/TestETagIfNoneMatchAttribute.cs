using EjemplosFormacion.WebApi.ParametersBindingAttributes.Abstract;
using EjemplosFormacion.WebApi.Stubs.Enums;

namespace EjemplosFormacion.WebApi.ParametersBindingAttributes
{
    class TestETagIfNoneMatchAttribute : ETagMatchParameterBindingAttribute
    {
        public TestETagIfNoneMatchAttribute() : base(TestETagMatchEnum.IfNoneMatch)
        {
        }
    }
}