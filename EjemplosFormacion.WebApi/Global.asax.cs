using System.Web.Http;

namespace EjemplosFormacion.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Ya que estamos usando Owin, no necesitamos esta clase para configurar nuestro HttpConfiguration del Web Api
            // En caso de no usar Owin, necesitas dejarla o fallara toda la aplicacion ya que no tendras Web Api por que no hay ninguna clase que llame a configurar la clase de Http Configuration
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
