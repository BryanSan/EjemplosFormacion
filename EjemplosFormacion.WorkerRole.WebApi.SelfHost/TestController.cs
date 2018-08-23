using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Net.Http;
using System.Web.Http;

namespace EjemplosFormacion.WorkerRole.WebApi.SelfHost
{
    /// <summary>
    /// Test Controller para devolver la informacion de la instancia donde halla llegado el Request
    /// </summary>
    public class TestController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string publicHttpIpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["PublicHttpEndpoint"].IPEndpoint?.ToString();
            string publicHttpPublicIpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["PublicHttpEndpoint"].PublicIPEndpoint?.ToString();

            string internalHttpIpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalHttpEndpoint"].IPEndpoint?.ToString();
            string internalHttpPublicIpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalHttpEndpoint"].PublicIPEndpoint?.ToString();
            
            string mensaje = string.Format(@"PublicHttpEndpoint => IPEndpoint : {0},  PublicIPEndpoint : {1}; {2}InternalHttpEndpoint => IPEndpoint : {3},  PublicIPEndpoint : {4};",
                                            publicHttpIpEndpoint, publicHttpPublicIpEndpoint, Environment.NewLine, internalHttpIpEndpoint, internalHttpPublicIpEndpoint);

            return new HttpResponseMessage()
            {
                Content = new StringContent(mensaje)
            };
        }
    }

}
