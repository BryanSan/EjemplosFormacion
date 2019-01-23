using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.WebApi.App_Start;
using EjemplosFormacion.WebApi.Authentication.BearerToken;
using EjemplosFormacion.WebApi.DependencyResolvers;
using EjemplosFormacion.WebApi.OwinAuthenticationHandlers;
using EjemplosFormacion.WebApi.OwinMiddlewares;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using Unity;

// Registro de la clase que sera usada para el StartUp del Owin, en este caso esta misma clase sera usada
[assembly: OwinStartup(typeof(Startup))]

namespace EjemplosFormacion.WebApi.App_Start
{
    public class Startup
    {
        // Metodo que sera llamado cuando la aplicacion inicie para configurar lo que necesite el Owin
        // En este caso estamos inicializando el SignalR
        public void Configuration(IAppBuilder app)
        {
            // Configuracion de servidor oAuth2
            // PRIMERO DEBES CONFIGURAR EL SERVIDOR DE OAUTH PORQUE SI NO LAS PETICIONES LLEGARAN UNAUTHENTICATED AL WEB API
            // RECORDAR QUE EL ORDEN ES SUPREMAMENTE IMPORTANTE EN WEB API
            // OJOOOOOOOOOOOOOOOOOOOOOOOOOO PROBLEMA MALDITO ROMPE VIDAS
            ConfigureOAuth(app);

            // Configuracion de Signal R
            app.MapSignalR();

            // Configuracion de Web Api
            RunWebApiConfiguration(app);

            // Configuracion de Custom Owin Middleware
            RegistroOwinMiddleware(app);
        }

        // Metodo para configurar Web Api
        // Simplemente crea una clase HttpConfiguration y usala para configurar el servicio Web Api
        // Esto es tal cual como si fuera un Web Api normal hosteado en IIS
        // Inclusive lo mas practico es crear la instancia de HttpConfiguration aqui, crear una clase WebApiConfig y pasarle la instancia a esa clase
        // Dentro de la clase WebApiConfig configura la instancia de HttpConfiguration como siempre
        // Al terminar de configurar la instancia de HttpConfiguration pasala al extension method appBuilder.UseWebApi(httpConfiguration) para habilitar el Web Api
        private void RunWebApiConfiguration(IAppBuilder app)
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
            // RECORDAR QUE ESTO AGREGA UN OWINMIDDLEWARE QUE HARA DE SERVICO WEB API
            // RECORDAD QUE EL ORDEN DE EJECUCION DE LOS OWINMIDDLEWARE ES POR ORDEN DE LLAMADA AL METODO EN CODIGO (ESTA CLASE)
            // SI HAY UN OWINMIDDLEWARE ANTES DE ESTE LLAMADO Y SON DE LOS QUE NO DEJAN PASAR NADA COMO EL RUN, NUNCA LLEGARAS AL WEB API
            // OJOOOOOOOOOOOOOOOOOOOOOOOOOO PROBLEMA MALDITO ROMPE VIDAS
            app.UseWebApi(httpConfiguration);
        }

        // Recordar que el metodo Run añade un Middleware sin continuacion, por esta razon al llegar al Middleware registrado por Run la ejecucion no seguira mas alla y por lo tanto no llegar a Web Api
        // El metodo Use añade un Middleware pero con continuacion por lo tanto puedes encadenar los Middleware para que procesen el Request y/o Response y podra llegar la ejecucion libremente a Web Api
        private void RegistroOwinMiddleware(IAppBuilder app)
        {
            // Esta propiedad solo estara en el AppBuilder si el Web Api esta siendo Self Host
            // Si esta siendo hosteada en IIS el HttpListener no existira
            if (app.Properties.ContainsKey("System.Net.HttpListener"))
            {
                // Para activar y modificar Authentication Schemas
                HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
                listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication | AuthenticationSchemes.Basic | AuthenticationSchemes.Anonymous;
            }

            // By default, OMCs run at the last event (PreHandlerExecute).
            // You can use the a app.UseStageMarker method to register a OMC to run earlier, at any stage of the OWIN pipeline listed in the PipelineStage enum
            // Debes configurar tu Middleware en orden ya que el orden que los registres seran el orden en el que se ejecutaran
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStageInIIS(context, "Middleware 1");
                return next.Invoke();
            });
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStageInIIS(context, "2nd MW");
                return next.Invoke();
            });

            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en el Trace todos los valores del Environment
            app.Use(async (env, next) =>
            {
                foreach (KeyValuePair<string, object> kvp in env.Environment)
                {
                    Trace.WriteLine(string.Concat("Key: ", kvp.Key, ", value: ", kvp.Value));
                }

                // Debes llamar a next para que la ejecucion pase al siguiente MiddleWare, similar a como son los Messaging Handlers de Web Api
                await next();
            });

            // Manera de crear un Middleware directo en la clase Startup sin necesidad de leer una clase
            // Solo printa en el Trace que metodos, que path y que status code tiene el HttpRequest y HttpResponse
            app.Use(async (env, next) =>
            {
                Trace.WriteLine(string.Concat("Http method: ", env.Request.Method, ", path: ", env.Request.Path));

                // Debes llamar a next para que la ejecucion pase al siguiente MiddleWare, similar a como son los Messaging Handlers de Web Api
                await next();

                Trace.WriteLine(string.Concat("Response code: ", env.Response.StatusCode));
            });

            // By default, OMCs run at the last event (PreHandlerExecute).
            // You can use the a app.UseStageMarker method to register a OMC to run earlier, at any stage of the OWIN pipeline listed in the PipelineStage enum
            // Si vas a cambiar en que stage del Pipeline en el que el Owin Middleware va a correr las subsequentes calls a app.UseStageMarker()
            // Deben estar en orden segun el orden del Pipeline, esto quiere decir que si configuro los 2 anteriores Middleware a correr en el Stage Authenticate
            // Los siguientes Middleware que registre mas abajo deben estar en los stages luego del stage de Authenticate, no pueden estar antes
            // IMPORTANTE EL ORDEN EN EL QUE LLAMAS LAS COSAS ACA
            app.UseStageMarker(PipelineStage.PreHandlerExecute);

            // Registro de Custom OwinMiddlewares creados heredando de la clase abstracta OwinMiddleware
            app.Use<TestSetOwinContextOwinMiddleware>();
            app.Use<TestRequestBufferingOwinMiddleware>();
            app.Use<TestOwinMiddleware>();

            // Custom Owin Middleware para validacion de Digital Certificates
            // Aqui pasamos las depedencias necesitadas por el AuthenticationMiddleware
            IDigitalCertificateValidator digitalCertificateValidator = UnityConfig.Container.Resolve(typeof(IDigitalCertificateValidator)) as IDigitalCertificateValidator;
            app.Use<TestClientCertificateAuthOwinMiddleware>(new TestClientCertificateAuthenticationOptions(), digitalCertificateValidator);

            // Codigo para mostrar una pagina de bienvenida cuando llegue una Http Request
            // El orden es importante, por tanto primero se mostrara la pagina de bienvenida antes que el Response Hard Coded ya que esta de primero
            // Digamos que agrega un Owin Middleware de primero
            // Este Middleware se insertara en el pipeline y no admitira mas Middleware subsiguientes
            app.UseWelcomePage();

            // Codigo para mostrar una Custom page cuando un Error es generado desde nuestro Owin Server
            app.UseErrorPage();
        }

        private void PrintCurrentIntegratedPipelineStageInIIS(IOwinContext context, string msg)
        {
            // Recordad que si el Web Api esta en modo Self Host la propiedad HttpContext.Current siempre sera null
            if (HttpContext.Current != null)
            {
                RequestNotification currentIntegratedpipelineStage = HttpContext.Current.CurrentNotification;
                context.Get<TextWriter>("host.TraceOutput").WriteLine("Current IIS event: " + currentIntegratedpipelineStage + " Msg: " + msg);
            }
        }

        // Configuracion de servidor oAuth2
        // RECORDAR QUE ESTE REGISTRO DEL oAuth2 OWINMIDLEWARE DEBE ESTAR ANTES QUE LA LLAMADA AL REGISTRO DE WEB API
        // O NUNCA SE AUTHENTICARA PRIMERO ANTES DE LLEGAR A LAS ACTION DE WEB API QUE PIDEN QUE YA ESTES AUTHENTICADO
        public void ConfigureOAuth(IAppBuilder app)
        {
            Type typeOfHasher = typeof(IHasher<SHA256Managed>);
            IHasher<SHA256Managed> hasher = UnityConfig.Container.Resolve(typeOfHasher) as IHasher<SHA256Managed>;
            ITokenGeneratorWithSymmetricKey tokenGeneratorWithSymmetricKey = UnityConfig.Container.Resolve(typeof(ITokenGeneratorWithSymmetricKey)) as ITokenGeneratorWithSymmetricKey;

            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"), // Path donde esta el endpoint para pedir los tokens, en este caso dominio + /token (http://localhost:7990/token)
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), // Expiracion del Bearer Token
                Provider = new TestSimpleAuthorizationServerProvider(hasher), // Servidor Provider de Bearer Tokens
                RefreshTokenProvider = new TestSimpleRefreshTokenAuthenticationProvider(hasher), // Servidor Provider de los Refresh Tokens
                AccessTokenFormat = new TestCustomJwtFormat(tokenGeneratorWithSymmetricKey), // Custom format to Access Jwt Tokens 
            };
            
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()); 
        }
    }
}