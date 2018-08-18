using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace EjemplosFormacion.WebApi.CORSPolices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    class TestCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

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

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}