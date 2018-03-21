using EjemplosFormacion.WebApi.ActionsFilters.NormalActionFilters;
using EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters.NormalActionFilters;
using EjemplosFormacion.WebApi.FiltersProviders;
using System.Web.Http;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi
{
    /// <summary>
    /// Clase usada para configurar el Web Api, tanto sus Rutas, Filtros y Servicios
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Registro de servicios
            ConfigureServices(config);

            // Registro de filtros globales
            ConfigureGlobalFilters(config);

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


        /// <summary>
        /// Cuidado al agregar servicios ya que se puede ejecutar una logica varias veces 
        /// Has un Replace para mantener 1 servicio de un solo tipo 
        /// Si haces un Add piensa bien si necesitas ambos servicios corriendo juntos
        /// Testea si es necesario para ver si la logica se ejecuta varias veces y no dañes el performance
        /// </summary>
        private static void ConfigureServices(HttpConfiguration config)
        {
            // Custom action filter provider which does ordering
            config.Services.Replace(typeof(IFilterProvider), new OrderedFilterProvider());
        }

        private static void ConfigureGlobalFilters(HttpConfiguration config)
        {
            // Orden de filtros a ejecutar
            // Primero consegue en que Scope esta y dentro de ese Scope que tipo de filtro es
            // Si hay multiple filtros que estan ubicados en el mismo nivel de orden no se garantiza el orden de ellos
            // Para esto debes crear una implementacion de un OrdererFilter para especificar de alguna manera el orden y un FilterProvider que lea estos orden especificados

            // Por Scope
            // 1 - Globals
            // 2 - Controllers 
            // 3 - Actions

            // Por tipo de filtros
            // 1 - Authorization filters
            // 2 - Action filters
            // 3 - Exception filters

            config.Filters.Add(new TestActionFilterAttribute()); // Normal Action Filter
            config.Filters.Add(new TestIActionFilterAttribute()); // Normal Action Filter
            config.Filters.Add(new TestOrderedActionFilterAttribute(order: 1)); // Normal Ordered Action Filter
        }
    }
}
