using EjemplosFormacion.HelperClasess.Abstract;
using EjemplosFormacion.WebApi.AssembliesResolver;
using EjemplosFormacion.WebApi.Controllers.TestCustomRoutesConfiguration;
using EjemplosFormacion.WebApi.CORSPolicyProviderFactories;
using EjemplosFormacion.WebApi.ExceptionHandlers;
using EjemplosFormacion.WebApi.ExceptionLoggers;
using EjemplosFormacion.WebApi.ExtensionMethods;
using EjemplosFormacion.WebApi.Filters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.AuthenticationFilters;
using EjemplosFormacion.WebApi.Filters.AuthorizationFilters;
using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ActionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.AuthorizationFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using EjemplosFormacion.WebApi.FiltersProviders;
using EjemplosFormacion.WebApi.HostBufferPolicySelectors;
using EjemplosFormacion.WebApi.HttpActionSelectors;
using EjemplosFormacion.WebApi.HttpControllerSelectors;
using EjemplosFormacion.WebApi.HttpControllerTypeResolvers;
using EjemplosFormacion.WebApi.HttpParametersBindings;
using EjemplosFormacion.WebApi.HttpRouteConstraints;
using EjemplosFormacion.WebApi.MediaTypeFormatters;
using EjemplosFormacion.WebApi.MessagingHandlers;
using EjemplosFormacion.WebApi.ModelBinders;
using EjemplosFormacion.WebApi.Stubs.Enums;
using EjemplosFormacion.WebApi.Stubs.Models;
using EjemplosFormacion.WebApi.TraceWriters;
using EjemplosFormacion.WebApi.ValueProviderFactories;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Http.Routing;
using System.Web.Http.Tracing;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi
{
    /// <summary>
    /// Clase usada para configurar el Web Api, tanto sus Rutas, Filtros y Servicios
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/configuring-aspnet-web-api
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Registro de Media Type Formatters
            ConfigureMediaTypeFormatters(config);

            // Parameter Binding Rules
            ConfigureParameterBindingRules(config);

            // Registro de servicios
            ConfigureServices(config);

            // Registro de filtros globales
            ConfigureGlobalFilters(config);

            // Registro Messaging Handlers
            ConfigureMessagingHandlers(config);

            // Registro de rutas que reconocera el Web Api
            ConfigureRoutes(config);

            // Enable Cross Origin Request
            ConfigureCors(config);
        }

        /// <summary>
        /// Custom Media Type Formatters para trabajar con Request que tengan en el Accept Header y/o Content Type Header
        /// Un MIME Type no soportado por Default por el Web API, añadiendo al servicio la posibilidad de aceptar mas formatos
        /// O de hacer override a las implementaciones que Web API tiene por default para los MIME Type que soporta Web API por default
        /// Que son json y xml
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/media-formatters
        /// </summary>
        /// <param name="config"></param>
        private static void ConfigureMediaTypeFormatters(HttpConfiguration config)
        {
            // Media Type Formatter para trabajar con Requeste con el formato application/atom+xml
            config.Formatters.Add(new TestAtomMediaTypeFormatter());

            // Media Type Formatter para trabajar con Request con el formato text/csv junto los encodings iso-8859-1 y UTF-8
            config.Formatters.Add(new TestCSVBufferedMediaTypeFormatter());
        }

        /// <summary>
        /// Custom Http Parameter Binding Rules que puedes usar para bindear un parametro de un Action con una Custom Logica
        /// Puedes usar el parameterDescriptor para revisar propiedades como el tipo del parametro y el HttpMethod usado por el Request y decidir que Http Parameter Binding usar
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        /// </summary>
        private static void ConfigureParameterBindingRules(HttpConfiguration config)
        {
            config.ParameterBindingRules.Add(parameterDescriptor =>
            {
                // Revisamos que el type del parametro del action sea del tipo soportado por el HttpParameterBinding que queremos usar
                // Y adicionalmente que el HttpMethod del Request sea Post
                if (parameterDescriptor.ParameterType == typeof(TestETagModel)
                    && parameterDescriptor.ActionDescriptor.SupportedHttpMethods.Contains(HttpMethod.Post))
                {
                    // Retornamos nuestra instancia de HttpParameterBinding para que Web Api la use para bindear el parametro que se esta evaluando
                    return new TestETagHttpParameterBinding(parameterDescriptor, TestETagMatchEnum.IfMatch);
                }
                else
                {
                    // Si no cumple nuestras condiciones o no queremos usar un Custom HttpParameterBinding para este parametro
                    // Devuelve null y asi Web Api sabra que no hay un Custom HttpParameterBinding para el parametro
                    return null;
                }
            });
        }

        /// <summary>
        /// Cuidado al agregar servicios ya que se puede ejecutar una logica varias veces 
        /// Has un Replace para mantener 1 servicio de un solo tipo 
        /// Si haces un Add piensa bien si necesitas ambos servicios corriendo juntos
        /// Testea si es necesario para ver si la logica se ejecuta varias veces y no dañes el performance
        /// O que por ejecutar la logica o pasar dos veces el servicio se dañen cosas
        /// Estos son puntos de extensibilidad para configurar a gusto el comportamiento y pipeline de Web API
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/configuring-aspnet-web-api
        /// </summary>
        private static void ConfigureServices(HttpConfiguration config)
        {
            // =========================================================
            //                  Single-Services
            // =========================================================

            // If an exception occurs, the IExceptionLogger will be called first,
            // Then the controller ExceptionFilters and if still unhandled, the IExceptionHandler implementation.
            // Exception handlers are the solution for customizing all possible responses to unhandled exceptions caught by Web API.
            config.Services.Replace(typeof(IExceptionHandler), new TestExceptionHandler());

            config.Services.Replace(typeof(IHostBufferPolicySelector), new NoWebHostBufferPolicySelector());

            // Custom Http Controller Selector para seleccionar un controller segun la version solicitada por el Request segun diferentes maneras
            // Query, Accept Header, Media Type Header
            // Usando el nombre del controller para comparar con la version solicitada
            // TestController y TestV2Controller son dos tipos que seran resueltos segun la version que venga 1 o 2
            config.Services.Replace(typeof(IHttpControllerSelector), new TestVersionControllerVersusControllerNameHttpControllerSelector(config));

            // Custom Http Controller Selector para seleccionar un controller segun la version solicitada por el Request segun diferentes maneras
            // Query, Accept Header, Media Type Header
            // Usando el NameSpace del controller para comparar con la version solicitada
            // TestController (NameSpace V1.TestController) y TestController (NameSpace V2.TestController) son dos tipos que seran resueltos segun la version que venga 1 o 2 y sus NameSpaces
            config.Services.Replace(typeof(IHttpControllerSelector), new TestVersionControllerVersusControllerNameSpaceHttpControllerSelector(config));

            // Custom Implementacion del servicio Web Api ITraceWriter para hacerle Trace a cada etapa del messaging pipeline
            // Web Api llamara el metodo Trace antes y despues de cada etapa del message pipeline, pasandole los parametros que el considere
            // Usalo para hacerle Trace (el de System.Diagnostics) a esta informacion y que los TraceListener lo persistan como consideren oportuno
            // O simplemente no le hagas Trace y persistelo tu mismo
            // POR DEFECTO WEB API NO TIENE UN ITRACEWRITER CONFIGURADO Y NO TRACEARA NADA, DEBES DAR TU PROPIA IMPLEMENTACION PARA QUE SE ACTIVE EL TRACING
            config.Services.Replace(typeof(ITraceWriter), new TestTraceWriter());

            // Custom Implementacion del servicio Web Api IHttpActionSelector para customizar como se selecciona el Action segun la Route que ha llegado
            config.Services.Replace(typeof(IHttpActionSelector), new TestHttpNotFoundActionSelector());

            // Custom Implementacion del servicio IAssembliesResolver en el cual Web Api se apoya para resolver cuales Assemblies son parte de su servicio
            // Por ejemplo a la hora de buscar los Controllers disponibles, Web Api se apoya en este servicio para obtener los Assembies en el cual buscar estos Controllers
            config.Services.Replace(typeof(IAssembliesResolver), new TestInternalAndExternalAssembliesResolver());

            // Custom Http Controller Type Resolver que se encargara de evaluar y seleccionar que clase seran las elegidas para servir de Controller
            // Puedes customizar tu criterio para elegir las clases que quieres que sirvan de Controller
            // En este caso estamos adicionando a los criterios ya establecidos que debe heredar de una clase base en concreto
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new TestIsDerivedFromBaseTypeHttpControllerTypeResolver());

            // =========================================================
            //                  Multi-Services
            // =========================================================

            // Custom action filter provider which does ordering
            // Se necesita que el Dependency Resolver resuelta y construya el tipo ya que se tiene una Dependencia al UnityContainer dentro del FilterProvider
            config.Services.Add(typeof(IFilterProvider), config.DependencyResolver.GetService(typeof(TestDependencyInjectionOrderedFilterProvider)));

            // If an exception occurs, the IExceptionLogger will be called first,
            // Then the controller ExceptionFilters and if still unhandled, the IExceptionHandler implementation.
            // Exception loggers are the solution to seeing all unhandled exception caught by Web API.
            // Se necesita que el Dependency Resolver resuelta y construya el tipo ya que se tiene una Dependencia al WrapperLoger dentro del ExceptionLogger
            config.Services.Add(typeof(IExceptionLogger), config.DependencyResolver.GetService(typeof(TestExceptionLogger)));

            // Custom Value Provider Factory que crea un Custom ValueProvider para obtener valores desde otra sources que no sea el Body o la URl (con Route Data)
            // Y posteriormente rellenar los parametros segun se requieran del Action que se va a invocar
            // Puedes llenar, cero, uno, varios o todos los parametros, ya queda de tu parte decidir
            // Como por ejemplo leer Headers y asignarle esos valores a los parametros del Action que se va a invocar
            // Recordar marcar los parametros del Action que se quieren llevar con el Attribute [ModelBinder]
            // Para indicarle al Web Api que ese parametro tiene un Custom ValueProvider y/o Custom Model Binder por atras que se encargara de llenarlo
            config.Services.Add(typeof(ValueProviderFactory), new TestHeaderValueProviderFactory());
            config.Services.Add(typeof(ValueProviderFactory), new TestCookieValueProviderFactory());

            // Custom Model Binder para rellenar un parametro Complex Type de un Action de un Controller con una custom logica
            // Como en este caso que se tiene un parametro con dos valores en el Query String separado por , que seran usado para llenar dos propiedades del Complext Type
            // Para registrar el Model Binder para un tipo puedes
            //     Colocarle al tipo que quieres soportar el Attribute ->
            //         [ModelBinder(typeof(TestModelBinder))]
            //     Colocar en el Action del Controller un Attribute junto con el Model Binder a usar ->
            //         [ModelBinder(typeof(TestModelBinder))] TestModelBinder.GeoPoint location
            //     Registrar el Model Binder en los Servicios del Web Api, insertandolo de PRIMERO ya que si lo insertas luego del Default, nunca sera llamado ya que el Default Model Binder ataja a todos los Types
            //     Y especificar en el Action del Controller que ese parametro sera llenado por un Model Binder o Value Provider ->
            //                     En el WebApiConfig.cs ->
            //                         var provider = new SimpleModelBinderProvider(typeof(TestModelBinder.GeoPoint), new TestModelBinder());
            //                         config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
            //                     En el Controller ->
            //                         [ModelBinder] TestModelBinder.GeoPoint location        
            var provider = new SimpleModelBinderProvider(typeof(TestModelBinder.GeoPoint), new TestModelBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider); // De primero o el Default Model Binder oscurece a todos tus Model Binder ya que ataja a todos los Types

            // Nueva manera de crear un ModelBinder para un tipo en concreto
            config.BindParameter(typeof(TestModelBinder.GeoPoint), new TestModelBinder());
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
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-and-action-selection
        /// </summary>
        private static void ConfigureRoutes(HttpConfiguration config)
        {
            // Clase usada para registrar los custom HttpRouteConstraint que hallas hecho, 
            // Y hacer la configuracion del match 1:1 con el nombre a usar en el Template Route ([Route("")]) con la custom HttpRouteConstraint hecha
            // En pocas palabras estar registrando tu Custom Route Constraint en el Web Api con un nombre para usarlo en el [Route("{id:intRange(1,10)}")]
            // De manera que cuando en el attribute [Route("{id:intRange(1,10)}")] uses el nombre de tu Route Constraint, Web Api sera capaz de resolverla
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("isSpecificValue", typeof(TestIsSpecificValueHttpRouteConstraint));
            constraintResolver.ConstraintMap.Add("nonZero", typeof(TestNonZeroHttpRouteConstraint));

            // Habilita el reconocimiento de rutas definidas como attributos en los controllers y actions
            // Las rutas definidas como atributos las configuras con el atributo [Route("ruta")] 
            // Y opcionalmente con prefijo con [RoutePrefix("prefijo")] quedando RoutePrefix + Route
            // Recordar que las rutas definidas en attributos se evaluan primero y sobreescriben las rutas definidas aqui en el global config
            // Este Custom Direct Route Provider agrega el string que le pases (api en este caso) a la ruta entregada por el attributo Route Prefix
            // Adicionalmente puedes pasar un InlineConstraintResolver para agregar tus Custom Http Route Constraints hechas por ti
            config.MapHttpAttributeRoutes(constraintResolver, config.DependencyResolver.GetService(typeof(IDirectRouteProvider)) as IDirectRouteProvider);
            // Puedes usar Route Constraints de igual manera en esta ruta, recordar que solo es un template  
            //config.MapHttpAttributeRoutes(constraintResolver, new TestDirectRouteProvider("api/v{version:int}"));

            // Se usa un Custom Direct Route Factory junto con el Custom Direct Route Provider y el metodo de extension RegisterTypedRoute 
            // Para crear Rutas de manera Type Safe y dejarlo de hacer con strings
            config.RegisterTypedRoute("TestTypedDirectRouteFactory", c => c.ConfigureRoute<TestTypedDirectRouteFactoryController>(x => x.TestTypedDirectRouteFactoryNoParams()));
            config.RegisterTypedRoute("TestTypedDirectRouteFactory/{id:int}", c => c.ConfigureRoute<TestTypedDirectRouteFactoryController>(x => x.TestTypedDirectRouteFactoryWithParams(default(int))));

            // Ruta para que tome en cuenta el nombre del action a la hora de evaluar y hacer match con la url del request
            config.Routes.MapHttpRoute(
                name: "RouteWithVersionAndActionName",
                routeTemplate: "api/v{version}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { version = @"\d+" }
            );

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

        // Configuracion de Filtros que seran aplicados a todo el servicio Web API
        // Pueden ser tanto de Authentication, Authorization, Exception 
        // O Action Filters normales para hacer un pre/post procesamiento de todas las Request recibidar por el Web API
        // Los Filters son parecidos a los Message Handlers pero se ejecutan despues en el Web API Pipeline
        // Segun sea los requerimientos o el gusto, puedes eleguir uno u otro
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

            // If an exception occurs, the IExceptionLogger will be called first,
            // Then the controller ExceptionFilters and if still unhandled, the IExceptionHandler implementation.
            // Exception filters are the easiest solution for processing the subset unhandled exceptions related to a specific action or controller.
            config.Filters.Add(new TestExceptionFilterAttribute()); // Excepcion Filter
            config.Filters.Add(new TestOrderedExceptionFilterAttribute(order: 1)); // Excepcion Action Filter - Primero en Ejecutar
            config.Filters.Add(new TestOrderedExceptionFilterAttribute(order: 2)); // Excepcion Action Filter - Segundo en Ejecutar

            config.Filters.Add(new TestBasicAuthenticationFilterAttribute()); // Authentication Filter with Basic Schema

            config.Filters.Add(new TestExtendedAuthorizeFilterAttribute()); // Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado)
            config.Filters.Add(new TestAuthorizationFilterAttribute()); // Authorize Filter 
            config.Filters.Add(new TestIAuthorizationFilterAttribute()); // Authorize Filter 
            config.Filters.Add(new TestOrderedAuthorizationFilterAttribute(order: 1)); // Authorize Filter 
            config.Filters.Add(new TestOrderedAuthorizationFilterAttribute(order: 2)); // Authorize Filter 

            // Authorization Filter para requerir solo Https
            //config.Filters.Add(new TestRequireHttpsAuthorizationFilterAttribute());

            // Authorization Filter para rederigir toda Request en Http a Https
            //config.Filters.Add(new TestRedirectHttpToHttpsAuthorizationFilterAttribute());

            // Se necesita que el Dependency Resolver resuelta y construya el tipo ya que se tiene una Dependencia al WrapperLoger dentro del ActionFilter
            config.Filters.Add(config.DependencyResolver.GetService(typeof(TestLoggingActionFilterAttribute)) as IFilter); // Action Filter with Dependency Property Injection

            // Web Api Build in Authorize Filter Requiere que el Request este autenticado (con un IPrincipal asignado), necesario para que si no tiene credenciales explote
            //config.Filters.Add(new AuthorizeAttribute()); // Authorize Filter 
        }

        /// <summary>
        /// Registro de Messaging Handlers
        /// Message Handlers son aplicador a todas las Request que lleguen al servicio Web API
        /// Te permiten realizar pre/post procesamiento de todas las Request recibidas por el Web API
        /// Los Message Handlers son parecidos a los Filters pero se ejecutan antes en el Web API Pipeline
        /// Segun sea los requerimientos o el gusto, puedes eleguir uno u otro
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-message-handlers
        /// </summary>
        /// <param name="config"></param>
        private static void ConfigureMessagingHandlers(HttpConfiguration config)
        {
            // Registro de Message Handler para todas las Request
            // Puedes pasar un Message Handler vacio, osea no hay cadena de Handlers, 
            // Si quieres que otro handler se ejecute luego de este, pasalo en el constructor y se creara la cadena 
            // O Agregalo a la coleccion de Message Handlers

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

            // Agregar el Pipeline de Message Handlers creado a la Route (Esta incluye el HttpControllerDispatcher)
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerWithFactory",
                routeTemplate: "api/TestMessagingHandler/TestWithMessagingHandlerWithFactory/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestWithMessagingHandlerWithFactory", id = RouteParameter.Optional },
                constraints: null,
                handler: routeHandlers // Message Handlers for this routes
            );

            // Puedes usar un Message especifico solo para una ruta sin cadena
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerNoChain",
                routeTemplate: "api/TestMessagingHandler/TestMessagingHandlerRouteSpecificNoChain/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMessagingHandlerRouteSpecificNoChain", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMessageHandler(new HttpControllerDispatcher(config)) // Message Handlers for this routes
            );

            // Puedes usar un Message especifico solo para una ruta con cadena
            config.Routes.MapHttpRoute(
                name: "RouteTestWithMessagingHandlerYesChain",
                routeTemplate: "api/TestMessagingHandler/TestMessagingHandlerRouteSpecificYesChain/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMessagingHandlerRouteSpecificYesChain", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMessageHandler(new TestMessageHandler(new HttpControllerDispatcher(config))) // Message Handlers for this routes
            );

            // Test Return Response Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestReturnResponseMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestReturnResponseMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReturnResponseMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestReturnResponseMessageHandler(new HttpControllerDispatcher(config)) // Message Handlers for this routes
            );

            // Test Method Override Header Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestMethodOverrideHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestMethodOverrideHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestMethodOverrideHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestMethodOverrideHeaderMessageHandler(new HttpControllerDispatcher(config)) // Message Handlers for this routes
            );

            // Test Add Header in Request and Response Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestAddHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestAddHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestAddHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestAddHeaderMessageHandler(new HttpControllerDispatcher(config)) // Message Handlers for this routes
            );

            // Test Search for key in Query String Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestSearchApiKeyMessagingHandler",
                routeTemplate: "api/TestMessagingHandler/TestReadQueryStringMessagingHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReadQueryStringMessagingHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestReadQueryStringMessagingHandler(new HttpControllerDispatcher(config), "key") // Message Handlers for this routes
            );

            // Test CRUD Cookies Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestCookiesMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestCookiesMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestCookiesMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestCookiesMessageHandler(new HttpControllerDispatcher(config)) // Message Handlers for this routes
            );

            // Test Reader Header Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestReadHeaderMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestReadHeaderMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestReadHeaderMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestReadHeaderMessageHandler(new HttpControllerDispatcher(config)) // Message Handler for this Route
            );

            // Test Basic Authentication Message Handler
            config.Routes.MapHttpRoute(
                name: "RouteTestBasicAuthenticatonMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestBasicAuthenticatonMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestBasicAuthenticatonMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestBasicAuthenticatonMessageHandler(new HttpControllerDispatcher(config)) // Message Handler for this Route
            );

            // Test Basic Authentication Message Handler that show a dialog box asking for credentials
            config.Routes.MapHttpRoute(
                name: "RouteTestBasicAuthenticatonMessageHandlerShowDialogBox",
                routeTemplate: "api/TestMessagingHandler/TestBasicAuthenticatonMessageHandlerShowDialogBox/",
                defaults: new { controller = "TestMessagingHandler", action = "TestBasicAuthenticatonMessageHandlerShowDialogBox", },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                handler: new TestBasicAuthenticatonMessageHandlerShowDialogBox(new HttpControllerDispatcher(config)) // Message Handler for this Route
            );

            // Test Decrypt Json Request and Encrypt Response to Json Response
            config.Routes.MapHttpRoute(
                name: "RouteTestJsonEncrypterMessageHandler",
                routeTemplate: "api/TestMessagingHandler/TestJsonEncrypterMessageHandler/{id}",
                defaults: new { controller = "TestMessagingHandler", action = "TestJsonEncrypterMessageHandler", id = RouteParameter.Optional },
                constraints: null,
                // Message Handler for this Route, necesitas el HttpControllerDispatcher ya que es el Handler que ejecuta al Controller
                // Message Handler for this Route
                handler: new TestJsonEncrypterMessageHandler(config.DependencyResolver.GetService(typeof(ISymmetricEncrypter<AesManaged, SHA256Managed>)) as ISymmetricEncrypter<AesManaged, SHA256Managed>, new HttpControllerDispatcher(config))
            );
        }

        // Habilita las Cross Origin Request, esto es hacer una peticion desde un navegador a otro dominio distinto al que esta alojado el Web Api
        // Esto es:
        //      http://example.net - Different domain
        //      http://example.com:9000/foo.html - Different port
        //      https://example.com/foo.html - Different scheme
        //      http://www.example.com/foo.html - Different subdomain
        // The origins parameter of the [EnableCors] attribute specifies which origins are allowed to access the resource. The value is a comma-separated list of the allowed origins. To allow all methods, use the wildcard value "*".
        // The headers parameter of the [EnableCors] attribute specifies which author request headers are allowed. To allow any headers, set headers to "*". Set headers to a comma-separated list of the allowed headers:
        // The methods parameter of the [EnableCors] attribute specifies which HTTP methods are allowed to access the resource.  The value is a comma-separated list of the allowed HTTP methods. To allow all methods, use the wildcard value "*".
        // If you set headers to anything other than "*", you should include at least "accept", "content-type", and "origin", plus any custom headers that you want to support.
        // By default, the browser does not expose all of the response headers to the application. The response headers that are available by default are:
        //      Cache-Control
        //      Content-Language
        //      Content-Type
        //      Expires
        //      Last-Modified
        //      Pragma
        // The CORS spec calls these simple response headers.To make other headers available to the application, set the exposedHeaders parameter of[EnableCors].
        // Credentials require special handling in a CORS request. By default, the browser does not send any credentials with a cross-origin request. Credentials include cookies as well as HTTP authentication schemes. To send credentials with a cross-origin request, the client must set XMLHttpRequest.withCredentials to true.
        // In addition, the server must allow the credentials. To allow cross-origin credentials in Web API, set the SupportsCredentials property to true on the [EnableCors] attribute
        // If this property is true, the HTTP response will include an Access-Control-Allow-Credentials header. This header tells the browser that the server allows credentials for a cross-origin request.
        // The CORS spec also states that setting origins to "*" is invalid if SupportsCredentials is true.
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

        private static void ConfigureCors(HttpConfiguration config)
        {
            // Enable CORS with a Custom CORS Policy Provider Factory
            config.SetCorsPolicyProviderFactory(new TestCorsPolicyProviderFactory());

            //      EnableCors configurado completo y cerrado al nivel Global
            //var cors = new EnableCorsAttribute(origins: "www.example.com", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header");
            //cors.SupportsCredentials = true;

            //      EnableCors configurado completo y abierto al nivel Global
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header");
            config.EnableCors(cors);
        }

    }
}