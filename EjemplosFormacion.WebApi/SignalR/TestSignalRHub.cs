using System.Threading.Tasks;
using EjemplosFormacion.WebApi.Models;
using Microsoft.AspNet.SignalR;

namespace EjemplosFormacion.WebApi.SignalR
{
    public class TestSignalRHub : Hub
    {
        
        // Este metodo sera publico para que cualquier cliente que se conecte a nuestro Hub pueda llamar Cliente -> Servidor
        public void TestMethodCallInServer()
        {
            // El metodo "TestMethodCallInClient" sera invocado por el servidor hacia el cliente Servidor - Cliente
            // En palabras sencillas cuando este metodo sea llamado, los clientes que se hallan registrado para escuchar este metodo "TestMethodCallInClient" 
            // Se ejecutara el CallBack, metodo o codigo (como le quieras decir) que hallan registrado para cuando se recibiera una peticion de este metodo "TestMethodCallInClient"

            // Se llamara "TestMethodCallInClient" para todos los Clientes registrados en el Hub sin ningun parametro que reciba el cliente
            Clients.All.TestMethodCallInClient();

            // Se llamara "TestMethodCallInClient" para todos los Clientes registrados en el Hub, con parametros que reciba el cliente (valor simple)
            Clients.All.TestMethodCallInClient("hola");

            // Se llamara "TestMethodCallInClient" para todos los Clientes registrados en el Hub, con parametros que reciba el cliente (objeto)
            TestModel test = new TestModel { Edad = 10, Nombre = "Hola" };
            Clients.All.TestMethodCallInClient(test);

            // Manera de obtener una referencia al HubContext especificado en el Generic Argument
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<TestSignalRHub>();
        }

        // Metodo llamado cuando un cliente se conecta a nuestro Hub
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        // Metodo llamado cuando un cliente se desconecta de nuestro Hub, sea por peticion explicita de el, por error de conexion o TimeOut
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        // Metodo llamado cuando un cliente se reconecta a nuestro Hub luego de un periodo de interrupcion
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }


    }
}