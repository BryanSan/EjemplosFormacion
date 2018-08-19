using EjemplosFormacion.WebApi.CORSPolices;
using System.Net.Http;
using System.Web.Http.Cors;

namespace EjemplosFormacion.WebApi.CORSPolicyProviderFactories
{
    /// <summary>
    /// Clase que actua de Factory para las Cors Policy Proviver 
    /// Implementa tu logica aqui y devuelve una instancia de CORS Policy Provider inspeccionando el Request que ha llegado
    /// El CORS Policy Provider configurara y devolvera el Policy segun lo implementes
    /// </summary>
    class TestCorsPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        // Metodo llamado por Web Api para hallar el CORS Policy Provider que configurara y devolvera el CORS Policy
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return new TestCorsPolicyAttribute();
        }
    }
}