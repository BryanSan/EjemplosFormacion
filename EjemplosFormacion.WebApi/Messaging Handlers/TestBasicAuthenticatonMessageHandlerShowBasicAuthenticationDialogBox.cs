using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para Autenticar Request con el Schema Basic, usando las credenciales pasadas en el Header de Authorization 
    /// En este caso siempre devolvera un Response con 401 de Unauthorized Y un Header de WWW-Authenticate Basic 
    /// Para que el cliente sepa que se esta intentando validar con el Schema Basic
    /// En el caso que el cliente sea un navegador y al ver que tiene un Header de WWW-Authenticate con el Schema Basic 
    /// Mostrara una alerta donde el usuario podra introducir sus credenciales y reenviara la peticion al servicio
    /// Recordar que el Header de WWW-Authenticate es usado para que el cliente sepa contra que Schema se esta validando y el sepa que tipo de credenciales enviar
    /// </summary>
    class TestBasicAuthenticatonMessageHandlerShowBasicAuthenticationDialogBox : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestBasicAuthenticatonMessageHandlerShowBasicAuthenticationDialogBox(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestBasicAuthenticatonMessageHandlerShowBasicAuthenticationDialogBox()
        {

        }

        // Metodo llamado por el Web Api cuando un Request es recibida y cuando un Response sera devuelta
        // Se diferencia por el antes y despues de la llamada al metodo base.SendAsync()
        // Antes del base.SendAsync() sera el Request, despues del base.SendAsync() sera el Response
        // Los Message Handler son parte del Pipeline de Asp.Net y te dan el chance de customizar, validar, etc el Response o Request
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Create a Unauthorized response with the header WWW-Authenticate and the schema used to authorizing
            // The browser will see the WWW-Authenticate and show a dialog box asking for credentials

            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            if (authorization == null || authorization.Scheme != AuthenticationSchemes.Basic.ToString())
            {
                // Create a Unauthorized response with the header WWW-Authenticate and the schema used to authorizing
                // The browser will see the WWW-Authenticate and show a dialog box asking for credentials
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Unauthorized);
                response.Headers.Add("WWW-Authenticate", "Basic");
                return response;
            }
            else
            {
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}