using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
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

            // Habilita el reconocimiento de rutas definidas como attributos en los controllers y actions
            // Recordar que las rutas definidas en attributos se evaluan primero y sobreescriben las rutas definidas aqui en el global config
            config.MapHttpAttributeRoutes();

            // Registro de rutas que reconocera el Web Api
            ConfigureRoutes(config);
        }


        /// <summary>
        /// Las rutas se evaluan comenzando desde la primera hasta la ultima
        /// Si una es evaluada y cumple con el patron no se evaluara mas rutas y se usara esa que ha cumplido
        /// Por regla general las rutas mas especificas van de primero y las generales de ultima
        /// Si ninguna ruta cumple lanzara una excepcion
        /// Puede darse el caso que varias rutas hagan match con la url del request, pero solo la primera que se evalua es la que se usara
        /// Recordar que las rutas definidas en attributos se evaluan primero y sobreescriben las rutas definidas aqui en el global config
        /// Donde uses {action} es reemplazado con el nombre del action que se esta evaluando, igualmente con {controller} con el nombre del controller que se esta evaluando
        /// Los pedazos de ruta marcado como opcional no seran necesarios que el url del request lo supla para decir que dicha url hizo match
        /// Los pedazos de ruta que no sean {controller} y {action} como {id} se anexaran al request 
        /// Seran recuperados por parametros que se llamen igual o parametros que sean objetos que tengan propiedades que se llamen igual 
        /// Ejemplo para {id} lo recuperas con int id o con un objeto con una propiedad que se llame id
        /// </summary>
        private static void ConfigureRoutes(HttpConfiguration config)
        {
            // Ruta para que tome en cuenta el nombre del action a la hora de evaluar y hacer match con la url del request
            config.Routes.MapHttpRoute(
                name: "RouteWithActionName",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Ruta default del Web Api, se centra al uso de los Http Verbs
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

            config.Filters.Add(new TestActionFilterAttribute()); // Action Filter
            config.Filters.Add(new TestIActionFilterAttribute()); // Action Filter
            config.Filters.Add(new TestOrderedActionFilterAttribute(order: 1)); // Ordered Action Filter - Primero en Ejecutar
            config.Filters.Add(new TestOrderedActionFilterAttribute(order: 2)); // Ordered Action Filter - Segundo en Ejecutar

            config.Filters.Add(new TestExceptionFilterAttribute()); // Excepcion Filter
            config.Filters.Add(new TestOrderedExceptionFilterAttribute(order: 1)); // Excepcion Action Filter - Primero en Ejecutar
            config.Filters.Add(new TestOrderedExceptionFilterAttribute(order: 2)); // Excepcion Action Filter - Segundo en Ejecutar

            config.Filters.Add(new TestBasicAuthenticationFilter()); // Authentication Filter with Basic Schema

            config.Filters.Add(new AuthorizeAttribute()); // Requiere que el Request este autenticado (con un IPrincipal asignado)
        }
    }
}