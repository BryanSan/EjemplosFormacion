using Microsoft.Owin;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.OwinMiddlewares
{
    /// <summary>
    ///  HttpContext.Current: This will be null. HttpContext is IIS based and will not be set when self-hosting with OWIN.. 
    /// If you have any code that relies on HttpContext, HttpRequest, or HttpResponse, it will have to be rewritten to handle an HttpRequestMessage or HttpResponseMessage, The HTTP types provided by Web API. 
    /// Fortunately, we still have access to CallContext provided by ASP.NET. This class can be used to provide per-request static semantics. 
    /// We have written an OWIN Middleware that gives us the request scope behavior of HttpContext.Current using CallContext:
    /// https://blog.uship.com/shipping-code/self-hosting-a-net-api-choosing-between-owin-with-asp-net-web-api-and-asp-net-core-mvc-1-0/
    /// Sets the current <see cref="IOwinContext"/> for later access via <see cref="OwinCallContext.Current"/>.
    /// Inspiration: https://github.com/neuecc/OwinRequestScopeContext
    /// </summary>
    public class TestSetOwinContextOwinMiddleware : OwinMiddleware
    {
        public TestSetOwinContextOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                OwinCallContext.Set(context);
                await Next.Invoke(context);
            }
            finally
            {
                OwinCallContext.Remove(context);
            }
        }
    }

    /// <summary>
    /// Helper class for setting and accessing the current <see cref="IOwinContext"/>
    /// </summary>
    public class OwinCallContext
    {
        private const string OwinContextKey = "owin.IOwinContext";

        public static IOwinContext Current
        {
            get { return (IOwinContext)CallContext.LogicalGetData(OwinContextKey); }
        }

        public static void Set(IOwinContext context)
        {
            CallContext.LogicalSetData(OwinContextKey, context);
        }

        public static void Remove(IOwinContext context)
        {
            CallContext.FreeNamedDataSlot(OwinContextKey);
        }
    }
}
