using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.OwinAuthenticationHandlers
{
    /// <summary>
    /// Valida un Digital Certificate que venga del Request usando uno de mis Helper Classes para hacer la validacion
    /// Si no hay certificado fallara, si lo hay intentara validarlo
    /// Si el certificado es valido, creo los Claims para el usuario, las Authentication Properties y devuelvo un Authentication Ticker con las dos informaciones
    /// </summary>
    public class TestOwinClientCertificateAuthenticationHandler : AuthenticationHandler<TestClientCertificateAuthenticationOptions>
    {
        readonly IDigitalCertificateValidator _digitalCertificateValidator;
        readonly string _owinClientCertKey = "ssl.ClientCertificate";

        public TestOwinClientCertificateAuthenticationHandler(IDigitalCertificateValidator digitalCertificateValidator)
        {
            if (digitalCertificateValidator == null) throw new ArgumentNullException("digitalCertificateValidator");
            _digitalCertificateValidator = digitalCertificateValidator;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            DigitalCertificateValidatorResult validationResult = await Task.Run(() => ValidateCertificate(Request.Environment));
            if (validationResult.Valid)
            {
                var authProperties = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                    AllowRefresh = true,
                    IsPersistent = true
                };

                var claimCollection = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Andras"),
                    new Claim(ClaimTypes.Country, "Sweden"),
                    new Claim(ClaimTypes.Gender, "M"),
                    new Claim(ClaimTypes.Surname, "Nemes"),
                    new Claim(ClaimTypes.Email, "hello@me.com"),
                    new Claim(ClaimTypes.Role, "IT"),
                    new Claim("HasValidClientCertificate", "true")
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimCollection, "X.509");
                AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, authProperties);
                return ticket;
            }
            return await Task.FromResult<AuthenticationTicket>(null);
        }

        private DigitalCertificateValidatorResult ValidateCertificate(IDictionary<string, object> owinEnvironment)
        {
            if (owinEnvironment.ContainsKey(_owinClientCertKey))
            {
                X509Certificate2 clientCert = Context.Get<X509Certificate2>(_owinClientCertKey);
                DigitalCertificateValidatorResult validationResult = _digitalCertificateValidator.Validate(clientCert, X509RevocationMode.Online, X509RevocationFlag.EntireChain, X509VerificationFlags.NoFlag);

                return validationResult;
            }
            else
            {
                var validationResult = new DigitalCertificateValidatorResult(false, "There's no client certificate attached to the request.");
                return validationResult;
            }
        }
    }

    public class TestClientCertificateAuthenticationOptions : AuthenticationOptions
    {
        public TestClientCertificateAuthenticationOptions() : base("X.509")
        { }
    }
}