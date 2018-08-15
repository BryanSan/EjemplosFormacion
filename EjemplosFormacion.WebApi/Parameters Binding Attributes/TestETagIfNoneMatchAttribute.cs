﻿using EjemplosFormacion.WebApi.ParametersBindingAttributes.Abstract;
using EjemplosFormacion.WebApi.Stubs.Enums;

namespace EjemplosFormacion.WebApi.ParametersBindingAttributes
{
    /// <summary>
    /// Custom Parameter Binding Attribute que usa una clase base para encapsular toda su logica
    /// Adorna un parameter de un Action y bindeara ese parametro con el HttpParameterBinding devuelvo por la clase base
    /// En este caso el HttpParameterBinding bindeara al parametro el valor del Header If-NoneMatch
    /// </summary>
    class TestETagIfNoneMatchAttribute : ETagMatchParameterBindingAttribute
    {
        public TestETagIfNoneMatchAttribute() : base(TestETagMatchEnum.IfNoneMatch)
        {
        }
    }
}