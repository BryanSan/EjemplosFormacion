using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;

namespace EjemplosFormacion.WebApi.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Aqui decidimos el url que se va a usar para escuchar las peticion http
            // Recordad que si este puerto ya esta en uso por cualquier otra aplicacion fallara
            string uri = "http://localhost:7990";
            string uri2 = "http://localhost:7991";
            string uri3 = "http://localhost:7992";

            // Puedes configurar la instancia de servidor en esta clase de StartOptions si quieres mas flexibilidad
            // O puedes configurar lo necesario directo en la llamada al metodo WebApp.Start(startOptions)) 
            StartOptions startOptions = new StartOptions();
            startOptions.Urls.Add(uri2);
            startOptions.Urls.Add(uri3);
            startOptions.AppStartup = typeof(Startup).FullName;

            // Crea la instancia de Owin Server especificando la clase usada para configurar el Owin Server y el Url que se usara para escuchar las peticiones Http
            // En este caso y por convencion es la clase Startup donde Owin pasara una instancia de IAppBuilder al metodo Configuration dando la oportunidad de configurar al Owin Server
            // Como es en este caso que aprovechamos para configurar una instancia de servicio de Web Api
            // Recordad que el host debe mantenerse activo, por esta razon el Console.ReadKey mantiene abierto la consola para que el Server siga activo y reciba peticiones
            // Puedes configurar la instancia de Owin server directo en la llamada o con una instancia de la clase StartOptions
            using (WebApp.Start(startOptions))
            //using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Web server on {0} starting.", uri);
                Console.ReadKey();
                Console.WriteLine("Web server on {0} stopping.", uri);
            }
        }
    }
}
