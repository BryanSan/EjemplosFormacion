using EjemplosFormacion.WebApi.DependencyResolvers;
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
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication | AuthenticationSchemes.Basic;

            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en la consola todos los valores del Environment
            appBuilder.Use(async (env, next) =>
            {
                foreach (KeyValuePair<string, object> kvp in env.Environment)
                {
                    Console.WriteLine(string.Concat("Key: ", kvp.Key, ", value: ", kvp.Value));
                }

                await next();
            });

            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en la consola que metodos, que path y que status code tiene el HttpRequest y HttpResponse
            appBuilder.Use(async (env, next) =>
            {
                Console.WriteLine(string.Concat("Http method: ", env.Request.Method, ", path: ", env.Request.Path));

                await next();

                Console.WriteLine(string.Concat("Response code: ", env.Response.StatusCode));
            });

            RunWebApiConfiguration(appBuilder);

            // Codigo para mostrar una pagina de bienvenida cuando llegue una Http Request
            // El orden es importante, por tanto primero se mostrara la pagina de bienvenida antes que el Response Hard Coded ya que esta de primero
            // Digamos que agrega un Owin Middleware de primero
            // Este Middleware se insertara en el pipeline y no admitira mas Middleware subsiguientes
            appBuilder.UseWelcomePage();

            // Codigo mostrar un mensaje cuando llegue una Http Request 
            // Este codigo escribira un Response con el mensaje
            // El orden es importante, por tanto primero se mostrara la pagina de bienvenida antes que el Response Hard Coded ya que esta de primero
            // Digamos que agrega un Owin Middleware de primero
            // Este Middleware se insertara en el pipeline y no admitira mas Middleware subsiguientes
            appBuilder.Run(owinContext =>
            {
                return owinContext.Response.WriteAsync("Hello from OWIN web server.");
            });
        }

        // Metodo para configurar Web Api
        // Simplemente crea una clase HttpConfiguration y usala para configurar el servicio Web Api
        // Esto es tal cual como si fuera un Web Api normal hosteado en IIS
        // Inclusive lo mas practico es crear la instancia de HttpConfiguration aqui, crear una clase WebApiConfig y pasarle la instancia a esa clase
        // Dentro de la clase WebApiConfig configura la instancia de HttpConfiguration como siempre
        // Al terminar de configurar la instancia de HttpConfiguration pasala al extension method appBuilder.UseWebApi(httpConfiguration) para habilitar el Web Api
        private void RunWebApiConfiguration(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            //httpConfiguration.Routes.MapHttpRoute(
            //    name: "WebApi", 
            //    routeTemplate: "{controller}/{id}", 
            //    defaults: new { id = RouteParameter.Optional }
            //);

            var resolver = new TestUnityDependencyResolver(UnityConfig.Container);
            httpConfiguration.DependencyResolver = resolver;

            WebApiConfig.Register(httpConfiguration);

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}
