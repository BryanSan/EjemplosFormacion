using EjemplosFormacion.HelperClasess.Abstract;
using EjemplosFormacion.WebApi.Authentication.BearerToken.Models;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    /// <summary>
    /// Refresh Token
    /// http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
    /// 
    /// Peticion hacia el endpoint configurado en el oAuth server en la clase Startup en este caso -> http://localhost:6719/token
    /// Body Peticion -> 
    ///     x-www-form-urlencoded con: 
    ///         grant_type:  refresh_token
    ///         refresh_token:  16339d743e6946b4845b0e542dd126f9
    ///         client_id:  WebApp
    ///         client_secret:T1m32014*
    /// 
    /// </summary>
    public class TestSimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public readonly IHasher<SHA256Managed> _hasher;

        public TestSimpleRefreshTokenProvider(IHasher<SHA256Managed> hasher)
        {
            _hasher = hasher;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).Wait();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (var _repo = new TestAuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new TestRefreshToken()
                {
                    Id = _hasher.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).Wait();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = _hasher.GetHash(context.Token);

            using (var _repo = new TestAuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }
    }
}