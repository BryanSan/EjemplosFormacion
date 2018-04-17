using System;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OverrideFilters
{
    /// <summary>
    /// Override filters disable higher-scoped filters of a given type.
    /// Use an override when you want to vary the filter pipeline for a single action method so that controller-level and global filters won’t be executed.
    /// Override filters do not affect filters applied at the same scope.
    /// Cuando definas el Type de la propiedad hazlo desde la Interfaz o Base Clase en su defecto o corres el riesgo que no se reconozca el Type a ignorar
    /// Ya hay Built in OverrideFilters, investiga a ver si alguno te sirve OverrideAuthenticationFilters, OverrideAuthorizationFilters, OverrideActionFilters, OverrideExceptionFilters   
    /// Puedes hacer una combinacion para ignorar el Attribute elegido que esta en un Scope mas alto y colocar uno nuevo con una configuracion diferente en el mismo Scope del OverrideFilter
    /// Ejemplo: En el controller Tienes un Autorize para los Admin y en el action uno para los Customer, 
    /// sin el override el usuario tendria que ser Admin y Customer para entrar, pero si haces override del Autorize en el controller este es ignorado y tendrias que ser solo Customer
    /// </summary>
    class TestOverrideFilterAttribute : Attribute, IFilter, IOverrideFilter
    {
        public bool AllowMultiple => false;

        Type _filtersToOverride;
        public Type FiltersToOverride
        {
            get { return _filtersToOverride; }
            private set { _filtersToOverride = value; }
        }

        public TestOverrideFilterAttribute(Type typeToOverride)
        {
            FiltersToOverride = typeToOverride;
        }
    }
}