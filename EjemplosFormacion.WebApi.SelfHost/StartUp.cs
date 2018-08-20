using EjemplosFormacion.WebApi.DependencyResolvers;
using EjemplosFormacion.WebApi.OwinMiddlewares;
using Owin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.SelfHost
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Para activar y modificar Authentication Schemas
            HttpListener listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication | AuthenticationSchemes.Basic | AuthenticationSchemes.Anonymous;

            // Configuracion y registro de WebApi
            RunWebApiConfiguration(appBuilder);

            // Configuracion de Owin Middlewares
            RegisterOwinMiddlewares(appBuilder);

            // Codigo mostrar un mensaje cuando llegue una Http Request 
            // Este codigo escribira un Response con el mensaje
            // El orden es importante, por tanto primero se mostrara la pagina de bienvenida antes que el Response Hard Coded ya que esta de primero
            // Digamos que agrega un Owin Middleware de primero
            // Este Middleware se insertara en el pipeline y no admitira mas Middleware subsiguientes
            // En este caso si Web Api no puede procesar el Request, el Request llegara aca y se generara un Response con un custom mensaje
            appBuilder.Run(owinContext =>
            {
                owinContext.Response.ContentType = "text/plain";
                return owinContext.Response.WriteAsync("Hello from OWIN web server.");
            });
        }

        private void RegisterOwinMiddlewares(IAppBuilder appBuilder)
        {
            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en la consola todos los valores del Environment
            appBuilder.Use(async (env, next) =>
            {
                foreach (KeyValuePair<string, object> kvp in env.Environment)
                {
                    Console.WriteLine(string.Concat("Key: ", kvp.Key, ", value: ", kvp.Value));
                }

                // Debes llamar a next para que la ejecucion pase al siguiente MiddleWare, similar a como son los Messaging Handlers de Web Api
                await next();
            });

            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en la consola que metodos, que path y que status code tiene el HttpRequest y HttpResponse
            appBuilder.Use(async (env, next) =>
            {
                Console.WriteLine(string.Concat("Http method: ", env.Request.Method, ", path: ", env.Request.Path));

                // Debes llamar a next para que la ejecucion pase al siguiente MiddleWare, similar a como son los Messaging Handlers de Web Api
                await next();

                Console.WriteLine(string.Concat("Response code: ", env.Response.StatusCode));
            });

            // Registro de Custom OwinMiddlewares creados heredando de la clase abstracta OwinMiddleware
            appBuilder.Use<TestSetOwinContextOwinMiddleware>();
            appBuilder.Use<TestRequestBufferingOwinMiddleware>();
            appBuilder.Use<TestOwinMiddleware>();

            // Codigo para mostrar una pagina de bienvenida cuando llegue una Http Request
            // El orden es importante, por tanto primero se mostrara la pagina de bienvenida antes que el Response Hard Coded ya que esta de primero
            // Digamos que agrega un Owin Middleware de primero
            // Este Middleware se insertara en el pipeline y no admitira mas Middleware subsiguientes
            appBuilder.UseWelcomePage();

            // Codigo para mostrar una Custom page cuando un Error es generado desde nuestro Owin Server
            appBuilder.UseErrorPage();
        }

        // Metodo para configurar Web Api
        // Simplemente crea una clase HttpConfiguration y usala para configurar el servicio Web Api
        // Esto es tal cual como si fuera un Web Api normal hosteado en IIS
        // Inclusive lo mas practico es crear la instancia de HttpConfiguration aqui, crear una clase WebApiConfig y pasarle la instancia a esa clase
        // Dentro de la clase WebApiConfig configura la instancia de HttpConfiguration como siempre
        // Al terminar de configurar la instancia de HttpConfiguration pasala al extension method appBuilder.UseWebApi(httpConfiguration) para habilitar el Web Api
        private void RunWebApiConfiguration(IAppBuilder appBuilder)
        {
            // Creamos nuestra propia instancia de HttpConfiguration para configurar nuestro servicio Web Api
            var httpConfiguration = new HttpConfiguration();

            // Como este proyecto es Self-Host, la clase que inicializa el Dependency Resolver en el otro proyecto
            // Nunca sera llamada y nunca tendremos un Dependency Resolver con UnityContainer
            // Por esta razon debemos crear a mano el Dependency Resolver junto con el UnityContainer y asignarlo manualmente a nuestra instancia de HttpConfiguration
            // Para que asi el pueda tener todas las Dependency registradas para posteriormente configurar y trabajar correctamente
            var resolver = new TestUnityDependencyResolver(UnityConfig.Container);

            // En este caso hacemos un truco y pasamos la instancia que acabamos de crear de HttpConfiguration al otro proyecto que tenemos mas completo para que configure la instancia de Web Api
            // De esta manera logramos hostear en este pequeño proyecto Owin todo el otro proyecto de Web Api
            httpConfiguration.DependencyResolver = resolver;

            // Mandamos a configurar nuestra insstancia de HttpConfiguration al otro proyecto mas completo asi hosteamos todo el otro proyecto en este proyecto Owin consola
            WebApiConfig.Register(httpConfiguration);

            // Le pasamos a Owin nuestra instancia configurada de HttpConfiguration para que inicie nuestro servicio Web Api
            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}
