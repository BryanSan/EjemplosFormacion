using EjemplosFormacion.WebApi.App_Start;
using Microsoft.Owin;
using Owin;

// Registro de la clase que sera usada para el StartUp del Owin, en este caso esta misma clase sera usada
[assembly: OwinStartup(typeof(Startup))]

namespace EjemplosFormacion.WebApi.App_Start
{
    public class Startup
    {
        // Metodo que sera llamado cuando la aplicacion inicie para configurar lo que necesite el Owin
        // En este caso estamos inicializando el SignalR
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
