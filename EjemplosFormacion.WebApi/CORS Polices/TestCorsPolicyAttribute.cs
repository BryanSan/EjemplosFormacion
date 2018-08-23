using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace EjemplosFormacion.WebApi.CORSPolices
{
    /// <summary>
    /// Custom CORS Policy Provider que se usara para adornar el Controller o Action al cual le aplicaremos las Policies
    /// Lo mismo que puedes hacer con el Attribute normal de CORS lo puedes hacer con un Custom de Policies
    /// Digamos que te sirve para tener la configuracion en una misma clase y asi poder reusarlo en vez de definirlo mil veces por todo el proyecto
    /// https://docs.microsoft.com/es-es/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    class TestCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        // Registra tu Policy aqui mejor para tener solo una sola instancia del Policy y no construir una cada vez que Web Api requiera una
        // Registra las mismas configuraciones como si estuvieras usando el CORS Attribute comun y corriente
        public TestCorsPolicyAttribute()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true,
            };

            _policy.ExposedHeaders.Add("X-Custom-Header");

            // Add Allowed Origins.
            _policy.Origins.Add("http://myclient.azurewebsites.net");
            _policy.Origins.Add("http://www.contoso.com");

            // Add Allowed Headers
            _policy.Headers.Add("Accept");
            _policy.Headers.Add("Content-Type");

            // Add Allowed Methods
            _policy.Methods.Add("GET");
            _policy.Methods.Add("POST");
        }

        // Metodo llamado por Web Api para obtener la Policy
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}