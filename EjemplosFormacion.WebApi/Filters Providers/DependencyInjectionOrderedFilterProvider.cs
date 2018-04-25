using EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

namespace EjemplosFormacion.WebApi.FiltersProviders
{
    /// <summary>
    /// Clase que implementa la interfaz IFilterProvider para servir de servicio en el Web Api para entregar filtros segun unos criterios
    /// Tener cuidado ya que el orden en el que devuelvas los filtros sera el orden en el que seran ejecutados segun su scope
    /// Tener cuidado de buscar en todos los scopes que filtros aplican para este Action Descriptor ya que si olvidas un scope o si filtras por un criterio erroneo
    /// Podrias ignorar algun filtro que si aplicaria a menos que otro FilterProvider lo agregue
    /// Si dos FilterProvider marcan el mismo filtro este filtro tendra tantas ejecuciones como se repita
    /// Valores bajos de la propiedad Order en los filtros marcados como ordenables son evaluados primeros
    /// </summary>
    class DependencyInjectionOrderedFilterProvider : IFilterProvider
    {
        private IUnityContainer _container;

        public DependencyInjectionOrderedFilterProvider(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentException("container vacio!.");
        }

        // Buscamos todos los filtros que aplican para este ActionDescriptor incluyendo Globales, Controllers y Actions Specific
        // Devolvemos todos aquellos que aplican para el, ordenados si estan marcados como ordenables
        // El orden en el cual devuelvas esta lista sera el orden que seran ejecutados segun su scope
        // Si no devuelves un filtro, no sera marcado para ejecutar y no se ejecutara
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // Global-specific
            IEnumerable<FilterInfo> globalSpecificFilters = OrderFilters(configuration.Filters.Select(x => x.Instance), FilterScope.Global);

            // Controller-specific
            IEnumerable<FilterInfo> controllerSpecificFilters = OrderFilters(actionDescriptor.ControllerDescriptor.GetFilters(), FilterScope.Controller);

            // Action-specific
            IEnumerable<FilterInfo> actionSpecificFilters = OrderFilters(actionDescriptor.GetFilters(), FilterScope.Action);

            // Concatena todos los filtros de todos los scopes o si no, no seran marcados para ejecutar por este FilterProvider
            // Si otro FilterProvider no los marca para ejecutar entonces seran ignorados
            // El orden en el cual devuelvas esta lista sera el orden que seran ejecutados segun su scope
            List<FilterInfo> listFilters = globalSpecificFilters.Concat(controllerSpecificFilters).Concat(actionSpecificFilters).ToList();

            // Se buildea los Filters con el Container de Dependency Injection por si alguno de los Filters necesita que se le injecte una dependency
            // Como algunos de los Filters que tienen Property Injection con el atributo [Dependency] en una propiedad
            listFilters.ForEach(r => _container.BuildUp(r.Instance.GetType(), r.Instance));

            return listFilters;
        }

        // Ordenamos los filtros marcados como ordenables a traves de la interfaz "IOrderedFilter" segun su parametro de orden
        // Los que no esten marcados como ordenables seran ejecutados despues de los que si estan marcados como ordenables
        // Valores bajos de la propiedad Order son evaluados primeros
        IEnumerable<FilterInfo> OrderFilters(IEnumerable<IFilter> filters, FilterScope scope)
        {
            // get all filter that dont implement IOrderedFilter and give them order number of 10000 to get executed last in his scope
            var notOrderableFilter = filters
                .Where(f => !(f is IOrderedFilter))
                .Select(instance => new KeyValuePair<int, FilterInfo>(10000, new FilterInfo(instance, scope)));

            // get all filter that implement IOrderedFilter and give them order number from the instance
            var orderableFilter = filters.OfType<IOrderedFilter>()
                .OrderBy(filter => filter.Order)
                .Select(instance => new KeyValuePair<int, FilterInfo>(instance.Order, new FilterInfo(instance, scope)));

            // concat lists => order => return
            return notOrderableFilter.Concat(orderableFilter)
                                     .OrderBy(x => x.Key)
                                     .Select(y => y.Value);
        }
    }
}