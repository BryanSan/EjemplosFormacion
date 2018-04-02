using EjemplosFormacion.WebApi.ActionResults;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthenticationFilters
{
    /// <summary>
    /// Authentication Filter usado para validar el Request, usando las credenciales otorgadas en el Authorization Header contra el Schema Basic
    /// Si no hay credenciales no hace nada
    /// Si tiene un Schema diferente al Basic no hace nada
    /// Si no hace nada no significa que aprueba ni que desautoriza, solo deja pasar y que otro vea que hace
    /// Usa una peticion con el header Authentication con valor Basic usuario:clave para poder probar
    /// En resumen valida las credenciales pasadas en el Request y asigna el IPrincipal y IIdentity al Request
    /// Puedes crear tus propias implementaciones para las Interfaces IPrincipal y IIdentity para guardar mas datos que necesites
    /// </summary>
    public class TestBasicAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != AuthenticationSchemes.Basic.ToString())
            {
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureActionResult("Missing credentials", request);
                return;
            }

            // Extracting credentials
            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
            if (userNameAndPasword == null)
            {
                context.ErrorResult = new AuthenticationFailureActionResult("Invalid credentials", request);
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            // Validating credentials and getting IPrincipal
            IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureActionResult("Invalid username or password", request);
            }

            // 6. If the credentials are valid, set principal.
            else
            {
                Thread.CurrentPrincipal = principal;
                context.Principal = principal;
            }

        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new AddChallengeOnUnauthorizedActionResult(AuthenticationSchemes.Basic, context.Result);
            return Task.FromResult(0);
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