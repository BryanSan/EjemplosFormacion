using EjemplosFormacion.WebApi.App_Start;
using EjemplosFormacion.WebApi.Authentication.BearerToken;
using EjemplosFormacion.WebApi.OwinMiddlewares;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.IO;
using System.Web;

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
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();

            RegistroOwinMiddleware(app);

            ConfigureOAuth(app);

            //var config = new HttpConfiguration();
            //var resolver = new TestUnityDependencyResolver(UnityConfig.Container);
            //httpConfiguration.DependencyResolver = resolver;

            //WebApiConfig.Register(config);
            //app.UseWebApi(config);
        }

        // Primero se ejecutan los Middleware y luego pasa la ejecucion al Web Api
        // Recordar que el metodo Run añade un Middleware sin continuacion, por esta razon al llegar al Middleware registrado por Run la ejecucion no seguira mas alla y por lo tanto no llegar a Web Api
        // El metodo Use añade un Middleware pero con continuacion por lo tanto puedes encadenar los Middleware para que procesen el Request y/o Response y podra llegar la ejecucion libremente a Web Api
        private void RegistroOwinMiddleware(IAppBuilder app)
        {
            // By default, OMCs run at the last event (PreHandlerExecute). That's why our first example code displayed "PreExecuteRequestHandler".
            // You can use the a app.UseStageMarker method to register a OMC to run earlier, at any stage of the OWIN pipeline listed in the PipelineStage enum
            // Debes configurar tu Middleware en orden ya que el orden que los registres seran el orden en el que se ejecutaran
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "Middleware 1");
                return next.Invoke();
            });
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "2nd MW");
                return next.Invoke();
            });

            // By default, OMCs run at the last event (PreHandlerExecute). That's why our first example code displayed "PreExecuteRequestHandler".
            // You can use the a app.UseStageMarker method to register a OMC to run earlier, at any stage of the OWIN pipeline listed in the PipelineStage enum
            // Si vas a cambiar en que stage del Pipeline en el que el Owin Middleware va a correr las subsequentes calls a app.UseStageMarker()
            // Deben estar en orden segun el orden del Pipeline, esto quiere decir que si configuro los 2 anteriores Middleware a correr en el Stage Authenticate
            // Los siguientes Middleware que registre mas abajo deben estar en los stages luego del stage de Authenticate, no pueden estar antes
            // IMPORTANTE EL ORDEN EN EL QUE LLAMAS LAS COSAS ACA
            app.UseStageMarker(PipelineStage.Authenticate);

            // Registro de Custom OwinMiddlewares creados heredando de la clase abstracta OwinMiddleware
            app.Use<TestSetOwinContextOwinMiddleware>();
            app.Use<TestRequestBufferingOwinMiddleware>();
            app.Use<TestOwinMiddleware>();
        }

        private void PrintCurrentIntegratedPipelineStage(IOwinContext context, string msg)
        {
            RequestNotification currentIntegratedpipelineStage = HttpContext.Current.CurrentNotification;
            context.Get<TextWriter>("host.TraceOutput").WriteLine("Current IIS event: " + currentIntegratedpipelineStage + " Msg: " + msg);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new TestSimpleAuthorizationServerProvider()
            };
            
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
