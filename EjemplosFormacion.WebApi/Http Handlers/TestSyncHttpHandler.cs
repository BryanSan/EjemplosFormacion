using System;
using System.Web;

namespace EjemplosFormacion.WebApi.HttpHandlers
{
    /// <summary>
    //  HTTP handlers are not get executed for every requests.
    //  It depends on the extension of resource we are requesting and known as extension based methodology.
    //  The most common handler is an ASP.NET page handler that processes.aspx files.
    //  When users request an .aspx file, the request is processed by the page through the page handler.
    //  We can create our own HTTP handlers that render custom output to the browser.
    //  In order to create a Custom HTTP Handler, we need to Implement IHttpHandler interface(synchronous handler) and Implement IHttpAsyncHandler(asynchronous handler).
    //  During the processing of an http request, only one HTTP handler will be called.
    //  In the asp.net request pipe line, HttpHandler comes after HttpModule and it is the end point objects in ASP.NET pipeline.
    /// </summary>
    class TestSyncHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            // To enable pooling, return true here.
            // This keeps the handler in memory.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // This handler is called whenever a file ending 
            // in .TestHttpHandlerFactory is requested. A file with that extension does not need to exist.
            // The path .TestHttpHandlerFactory is configured in the registration of this HttpHandler in the Web.Config
            context.Response.Write("Hello from Sync Handler!" + "\r\n");
            context.Response.Write("EndProcessRequest.");
        }
    }
}