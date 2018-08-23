using Microsoft.Owin;
using System.IO;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.OwinMiddlewares
{
    /// <summary>
    /// HttpRequestMessage.Content.ReadAsStreamAsync().Result: IIS let’s you read the request stream multiple times, 
    /// But by default OWIN does not, not does it let you reset the stream after reading it once. 
    /// A common reason people need to read the stream twice is to log the incoming request before the input body is deserialized by the framework. 
    /// We have written an OWIN Middleware that copies the request stream into an in-memory buffer to get around this:
    /// https://blog.uship.com/shipping-code/self-hosting-a-net-api-choosing-between-owin-with-asp-net-web-api-and-asp-net-core-mvc-1-0/
    /// </summary>
    public class TestRequestBufferingOwinMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Buffers the request stream to allow for reading multiple times.
        /// The Katana (OWIN implementation) implementation of request streams
        /// is different than that of IIS.
        /// </summary>
        public TestRequestBufferingOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }

        // Explanation of why this is necessary: http://stackoverflow.com/a/25607448/4780595
        // Implementation inspiration: http://stackoverflow.com/a/26216511/4780595
        public override Task Invoke(IOwinContext context)
        {
            var requestStream = context.Request.Body;
            var requestMemoryBuffer = new MemoryStream();

            requestStream.CopyTo(requestMemoryBuffer);
            requestMemoryBuffer.Seek(0, SeekOrigin.Begin);

            context.Request.Body = requestMemoryBuffer;

            return Next.Invoke(context);
        }
    }
}
