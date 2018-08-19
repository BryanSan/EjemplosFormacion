using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.OwinMiddlewares
{
    /// <summary>
    /// Custom Owin Middleware que actua como si fuera Messaging Handler de Web Api
    /// Configura el Request y Response segun creas pertinente con tu Custom Logica
    /// Primero corren los Owin Middleware y despues corre Web Api
    /// </summary>
    public class TestOwinMiddleware : OwinMiddleware
    {

        public TestOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            // Obtiene un Owin Environment
            HttpListener httpListener = context.Get<HttpListener>("System.Net.HttpListener");

            // Asigna un Owin Environment
            context.Set<HttpListener>("System.Net.HttpListener", httpListener);

            // Recorre los Owin Environment
            foreach (KeyValuePair<string, object> kvp in context.Environment)
            {

            }
            
            // Authentication Manager del Request
            IAuthenticationManager authentication = context.Authentication;

            // Request de la peticion
            IOwinRequest request = context.Request;

            await Next.Invoke(context);

            // Respnse de la peticion
            IOwinResponse response = context.Response;
        }
    }
}