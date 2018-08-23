using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WorkerRole.WebApi.SelfHost
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IDisposable _app = null;

        public override void Run()
        {
            Trace.TraceInformation("EjemplosFormacion.WorkerRole.WebApi.SelfHost is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        // Configuracion e inicio de una instancia de Web Api Self-Host en este Worker Role
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            // Hallamos el Endpoint configurado para este Rol
            // Este enpoint es configurado en el proyecto EjemplosFormacion.CloudService, seleccionando este Rol y registrandole Endpoints
            // Puedes registrar tanto Public, Internal y InstanceEndpoints para comunicaciones publicas, internas entre roles y comunicaciones directas hacia una instancia en especifica
            RoleInstanceEndpoint endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["PublicHttpEndpoint"];

            // Una ves tenido el endpoint armas la url base donde tu servidor Web Api sera accesible y estara escuchando
            string baseUri = string.Format("{0}://{1}", endpoint.Protocol, endpoint.IPEndpoint);

            Trace.TraceInformation(string.Format("Starting OWIN at {0}", baseUri), "Information");

            // Inicializas el Web Api SelfHost
            _app = WebApp.Start<Startup>(new StartOptions(url: baseUri));

            bool result = base.OnStart();

            Trace.TraceInformation("EjemplosFormacion.WorkerRole.WebApi.SelfHost has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("EjemplosFormacion.WorkerRole.WebApi.SelfHost is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            if (_app != null)
            {
                _app.Dispose();
            }

            base.OnStop();

            Trace.TraceInformation("EjemplosFormacion.WorkerRole.WebApi.SelfHost has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
