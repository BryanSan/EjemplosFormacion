using EjemplosFormacion.WebApi.CORSPolices;
using System.Net.Http;
using System.Web.Http.Cors;

namespace EjemplosFormacion.WebApi.CORSPolicyProviderFactories
{
    class TestCorsPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return new TestCorsPolicyAttribute();
        }
    }
}