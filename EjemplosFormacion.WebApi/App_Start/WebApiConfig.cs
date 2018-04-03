using EjemplosFormacion.HelperClasess;
using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using EjemplosFormacion.WebApi.Filters.AuthorizationFilters;
using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.AuthorizationFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using EjemplosFormacion.WebApi.FiltersProviders;
using EjemplosFormacion.WebApi.IoC;
using EjemplosFormacion.WebApi.MessagingHandlers;
using EjemplosFormacion.WebApi.Stubs.Abstract;
using EjemplosFormacion.WebApi.Stubs.Implementation;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

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

            //Configure Dependency Resolver (IoC Bridge)
            ConfigureDependencyResolver(config);

            // Registro de servicios
            ConfigureServices(config);

            // Registro de filtros globales
            ConfigureGlobalFilters(config);

            // Registro Messaging Handlers
            ConfigureMessagingHandlers(config);

            // Registro de rutas que reconocera el Web Api
            ConfigureRoutes(config);
        }

        /// <summary>
        /// Se configura el Dependency Container y se registrar en el Dependency Resolver del Web Api
        /// </summary>  
        private static void ConfigureDependencyResolver(HttpConfiguration config)
        {
            // Se crea el IoC Container
            UnityContainer container = new UnityContainer();

            // Se registran las dependencias en el IoC Container
            RegisterDependencies(container);

            // Se registra el Dependency Resolver y el IoC Container a usar
            config.DependencyResolver = new UnityDependencyResolver(container);
        }

        /// <summary>
        /// Se registran las dependencias de la aplicacion
        /// </summary>
        private static void RegisterDependencies(UnityContainer container)
        {
            // Registrar tus dependencias
            container.RegisterType<ITestDependency, TestDependency>(new HierarchicalLifetimeManager());

            TestDependency testDependency = new TestDependency();
            container.RegisterInstance<ITestDependency>(testDependency);
            container.RegisterType<TestWithDependencyActionFilterAttribute>(new InjectionProperty("Dependency"));
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
            // Habilita el reconocimiento de rutas definidas como attributos en los controllers y actions
            // Recordar que las rutas definidas en attributos se evaluan primero y sobreescriben las rutas definidas aqui en el global config
            config.MapHttpAttributeRoutes();

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

            config.Filters.Add(new TestBasicAuthenticationFilterAttribute()); // Authentication Filter with Basic Schema

            config.Filters.Add(new TestExtendedAuthorizeFilterAttribute()); // Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado)
            config.Filters.Add(new TestAuthorizationFilterAttribute()); // Authorize Filter 
            config.Filters.Add(new TestIAuthorizationFilterAttribute()); // Authorize Filter 
            config.Filters.Add(new TestOrderedAuthorizationFilterAttribute(order: 1)); // Authorize Filter 
            config.Filters.Add(new TestOrderedAuthorizationFilterAttribute(order: 2)); // Authorize Filter 

            // Web Api Build in Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado), necesario para que si no tiene credenciales explote
            //config.Filters.Add(new AuthorizeAttribute()); // Authorize Filter 
        }

        // Registro de Messaging Handlers
        private static void ConfigureMessagingHandlers(HttpConfiguration config)
        {
            // Registro de Message Handler para todas las Request
            // Puedes pasar un Message Handler vacio, osea no hay cadena de Handlers, 
            // Si quieres que otro handler se ejecute luego de este, pasalo en el constructor y se creara la cadena 

            // Registro Messaging Handler sin cadena (Sin otro Messaging Handler que le siga)
            // Agregar uno a uno cada Message Handler
            config.MessageHandlers.Add(new TestMessageHandler());

            // Registro Messaging Handler con cadena (Le sigue otro Messaging Handler)
            // Esto no sirve en este contexto ya que si necesitas que otro Message Handler le siga debes hacer dos Add y no hacerlo de esta manera
            // Ya que cuando hagas una peticion te dara error 
            // La lista 'DelegatingHandler' no es valida porque la propiedad 'InnerHandler' de 'TestMessageHandler' no es nula.
            // config.MessageHandlers.Add(new TestMessageHandler(new TestMessageHandler()));

            // Puedes definir una lista de handlers de esta manera
            DelegatingHandler[] handlers = new DelegatingHandler[] { new TestMessageHandler() };
            // Crear una Pipeline con tus handlers + el Handler que ejecuta el Controller y lo puedes asignar al handler de tu ruta
            HttpMessageHandler routeHandlers = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(config), handlers);

            // Agregar el Pipeline creado a la ruta
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerWithFactory",
                routeTemplate: "api/TestMessagingHandler/TestWithMessagingHandlerWithFactory/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestWithMessagingHandlerWithFactory", id = RouteParameter.Optional },
                constraints: null,
                handler: routeHandlers
            );

            // Puedes usar un Message especifico solo para una ruta sin cadena
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerNoChain",
                routeTemplate: "api/TestMessagingHandler/TestMessagingHandlerRouteSpecificNoChain/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMessagingHandlerRouteSpecificNoChain", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMessageHandler(new HttpControllerDispatcher(config))
            );

            // Puedes usar un Message especifico solo para una ruta con cadena
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerYesChain",
                routeTemplate: "api/TestMessagingHandler/TestMessagingHandlerRouteSpecificYesChain/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMessagingHandlerRouteSpecificYesChain", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMessageHandler(new TestMessageHandler(new HttpControllerDispatcher(config)))
            );

            // Test Return Response Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestReturnResponseMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestReturnResponseMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReturnResponseMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestReturnResponseMessageHandler(new HttpControllerDispatcher(config))
            );

            // Test Method Override Header Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestMethodOverrideHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestMethodOverrideHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMethodOverrideHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMethodOverrideHeaderMessageHandler(new HttpControllerDispatcher(config))
            );

            // Test Add Header in Request and Response Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestAddHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestAddHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestAddHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestAddHeaderMessageHandler(new HttpControllerDispatcher(config))
            );

            // Test Search for key in Query String Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestSearchApiKeyMessagingHandler",
                routeTemplate: "api/TestMessagingHandler/TestReadQueryStringMessagingHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReadQueryStringMessagingHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestReadQueryStringMessagingHandler(new HttpControllerDispatcher(config), "key")
            );

            // Test CRUD Cookies Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestCookiesMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestCookiesMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestCookiesMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestCookiesMessageHandler(new HttpControllerDispatcher(config))
            );

            // Test Reader Header Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestReadHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestReadHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReadHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                handler: new TestReadHeaderMessageHandler(new HttpControllerDispatcher(config)) // Message Handler for this Route
            );

            // Test Basic Authentication Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestBasicAuthenticatonMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestBasicAuthenticatonMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestBasicAuthenticatonMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestBasicAuthenticatonMessageHandler(new HttpControllerDispatcher(config))
            );

            // Test Decrypt Json Request and Encrypt Response to Json Response
            config.Routes.MapHttpRoute(
                name: "RouteTestJsonEncrypterMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestJsonEncrypterMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestJsonEncrypterMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestJsonEncrypterMessageHandler(new SymmetricEncrypter<AesManaged, SHA256Managed>(), new HttpControllerDispatcher(config))
            );
        }

    }
}