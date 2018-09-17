using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Custom Message Handler que revisa el Request por un Client Certificate
    /// Puedes hacer una logica para validarlo y decidir si devolver un Response con Error o dejarlo pasar
    /// Puedes usarlo para authenticar tambien
    /// 
    /// Asi anexas desde el cliente al request un Certificate
    /// X509Certificate2 clientCert = GetClientCertificate();
    /// WebRequestHandler requestHandler = new WebRequestHandler();
    /// requestHandler.ClientCertificates.Add(clientCert);
    ///
    /// HttpClient client = new HttpClient(requestHandler)
    // {
    //     BaseAddress = new Uri("http://localhost:3020/")
    // };
    /// </summary>
    public class TestReadClientCertificateMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestReadClientCertificateMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {
            if (messageHandler == null) throw new ArgumentException("messageHandler vacio!.");
        }

        public TestReadClientCertificateMessageHandler()
        {

        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Recuperamos el Client Certificate
            X509Certificate2 clientCertificate = request.GetClientCertificate();
            string issuer = clientCertificate.Issuer;
            string subject = clientCertificate.Subject;

            // Validamos que el Client Certificate exista y este valido
            // Esta validacion varia segun las reglas de negocio, algunos podran validar que el public key concuerde
            // Otros que el issuer, todo depende de las reglas del negocio
            if (clientCertificate == null || !clientCertificate.Verify())
            {
                // Si no devolvemos un Error de Unauthorized
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid certificate!.");
                return Task.FromResult(response);
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}