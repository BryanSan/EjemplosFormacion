using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Authorization Filter usado para validar que el client halla enviado un Certificate junto con el Request
    /// Y que el Certificate tambien sea valido, si no devuelve un error de Unauthorized
    /// </summary>
    public class TestReadClientCertificateAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Recuperamos el Request
            HttpRequestMessage requestMessage = actionContext.Request;

            // Recuperamos el Client Certificate
            X509Certificate2 clientCertificate = requestMessage.GetClientCertificate();

            // Validamos que el Client Certificate exista y este valido
            // Esta validacion varia segun las reglas de negocio, algunos podran validar que el public key concuerde
            // Otros que el issuer, todo depende de las reglas del negocio
            if (clientCertificate == null || !clientCertificate.Verify())
            {
                // Si no devolvemos un Error de Unauthorized
                actionContext.Response = requestMessage.CreateResponse(HttpStatusCode.Unauthorized, "Invalid certificate!.");
            }
        }
    }
}