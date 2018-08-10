using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para Autenticar Request con el Schema Basic, usando las credencialas pasadas en el Header de Authorization 
    /// Si no tiene header, o credenciales deja pasar sin problemas mas no lo marca como autenticado (asigna el IPrincipal)
    /// Si tiene header y credenciales y no son validas, tanto en formato como en Autenticacion devuelve un error junto con el Challenger Header para exponer contra que se esta validando
    /// </summary>
    class TestBasicAuthenticatonMessageHandlerShowDialogBox : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestBasicAuthenticatonMessageHandlerShowDialogBox(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestBasicAuthenticatonMessageHandlerShowDialogBox()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Create a Unauthorized response with the header WWW-Authenticate and the schema used to authorizing
            // The browser will see the WWW-Authenticate and show a dialog box asking for credentials
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Unauthorized);
            response.Headers.Add("WWW-Authenticate", "Basic");
            return Task.FromResult(response);
        }
    }
}