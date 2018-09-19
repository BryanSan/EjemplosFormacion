using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.WebApi.OwinAuthenticationHandlers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using System;

namespace EjemplosFormacion.WebApi.OwinMiddlewares
{
    /// <summary>
    /// Solo es un Authentication Middleware que recibe el tipo de Authentication que va a soportar
    /// Luego el creara los Handlers (Que es quien implementa la logica de authentication de verdad) para authenticar el Request
    /// Miralo como una Factory de un tipo de Authentication
    /// </summary>
    public class TestClientCertificateAuthOwinMiddleware : AuthenticationMiddleware<TestClientCertificateAuthenticationOptions>
    {
        readonly IDigitalCertificateValidator _digitalCertificateValidator;

        // Recuerda que el OwinMiddleware es filleado automaticamente por .Net, por lo tanto tu solo debes preocuparte por el 2do y subsiguientes
        // En el momento que los registres en la clase Startup pasa las instancias necesitadas por este constructor en el metodo Use()
        public TestClientCertificateAuthOwinMiddleware(OwinMiddleware nextMiddleware, TestClientCertificateAuthenticationOptions authOptions,
            IDigitalCertificateValidator digitalCertificateValidator)
            : base(nextMiddleware, authOptions)
        {
            if (digitalCertificateValidator == null) throw new ArgumentNullException("digitalCertificateValidator");
            _digitalCertificateValidator = digitalCertificateValidator;
        }

        protected override AuthenticationHandler<TestClientCertificateAuthenticationOptions> CreateHandler()
        {
            return new TestOwinClientCertificateAuthenticationHandler(_digitalCertificateValidator);
        }
    }
}