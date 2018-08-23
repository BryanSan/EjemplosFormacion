using EjemplosFormacion.WorkerRole.WebApi.SelfHost;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace EjemplosFormacion.WorkerRole.WebApi.SelfHost
{
    /// <summary>
    /// Startup Class para configurar el servidor Owin
    /// En este caso estamos configurando el Web Api y habilitandolo en el servidor Owin
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "Default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            app.UseWebApi(config);

        }
    }
}
