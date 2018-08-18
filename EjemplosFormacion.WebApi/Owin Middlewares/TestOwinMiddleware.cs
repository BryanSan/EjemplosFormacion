using Microsoft.Owin;
using System.Net;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.OwinMiddlewares
{
    public class TestOwinMiddleware : OwinMiddleware
    {

        public TestOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            HttpListener httpListener = context.Get<HttpListener>("System.Net.HttpListener");
            context.Set<HttpListener>("System.Net.HttpListener", httpListener);

            return Next.Invoke(context);
        }
    }
}