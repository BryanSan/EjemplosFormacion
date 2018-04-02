using EjemplosFormacion.WebApi.ActionResults;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler para Autenticar Request con el Schema Basic, usando las credencialas pasadas en el Header de Authorization 
    /// Si no tiene header, o credenciales deja pasar sin problemas mas no lo marca como autenticado (asigna el IPrincipal)
    /// Si tiene header y credenciales y no son validas, tanto en formato como en Autenticacion devuelve un error junto con el Challenger Header para exponer contra que se esta validando
    /// </summary>
    public class TestBasicAuthenticatonMessageHandler : DelegatingHandler
    {
        // Passing the next Handler of the Pipeline If Any
        public TestBasicAuthenticatonMessageHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        public TestBasicAuthenticatonMessageHandler()
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Before Execute Action
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null || authorization.Scheme != AuthenticationSchemes.Basic.ToString())
            {
                return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                // 4. If there are credentials that the filter understands, try to validate them.
                // 5. If the credentials are bad, set the error result.
                if (string.IsNullOrEmpty(authorization.Parameter))
                {
                    AuthenticationFailureActionResult errorActionResult = new AuthenticationFailureActionResult("Missing credentials", request);
                    return await errorActionResult.ExecuteAsync(cancellationToken);
                }

                // Extracting credentials
                Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
                if (userNameAndPasword == null)
                {
                    AuthenticationFailureActionResult errorActionResult = new AuthenticationFailureActionResult("Invalid credentials", request);
                    return await errorActionResult.ExecuteAsync(cancellationToken);
                }

                string userName = userNameAndPasword.Item1;
                string password = userNameAndPasword.Item2;

                // Validating credentials and getting IPrincipal
                IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);
                if (principal == null)
                {
                    AuthenticationFailureActionResult errorActionResult = new AuthenticationFailureActionResult("Invalid username or password", request);
                    return await errorActionResult.ExecuteAsync(cancellationToken);
                }

                // 6. If the credentials are valid, set principal.
                else
                {
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;
                }

                // Pasa el procesamiento al siguiente handler, si no hay mas message handler va al Action destino
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                // Si no fue autorizada el Request, entonces añadimos el header de WwwAuthenticate para que el cliente sepa con que Schema se valido
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    AddChallengeOnUnauthorizedActionResult challengeOnUnauthorized = new AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes.Basic, response);
                    return await challengeOnUnauthorized.ExecuteAsync(cancellationToken);
                }
                else
                {
                    return response;
                }
            }
        }

        // Extrae las credenciales (Username y Password)
        Tuple<string, string> ExtractUserNameAndPassword(string credentials)
        {
            // Si las credenciales estan en Base64 hazle decode
            //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            //credentials = encoding.GetString(Convert.FromBase64String(credentials));

            int separator = credentials.IndexOf(':');
            string name = credentials.Substring(0, separator);
            string password = credentials.Substring(separator + 1);

            return Tuple.Create(name, password);
        }

        // Authentica el usuario, halla los roles, crea el IPrincipal y devuelvelo
        Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            if (AuthenticateInDataBase())
            {
                string[] roles = GetRoles(userName);

                GenericIdentity identity = new GenericIdentity(userName);
                IPrincipal principal = new GenericPrincipal(identity, roles);

                return Task.FromResult(principal);
            }
            else
            {
                return null;
            }
        }

        // Coloca la logica que necesitas para authenticar en el DataBase u otro sitio
        bool AuthenticateInDataBase()
        {
            return true;
        }

        // Coloca la logica que necesitas para hallar los roles del usuario
        string[] GetRoles(string userName)
        {
            return new string[] { "admin" };
        }
    }
}