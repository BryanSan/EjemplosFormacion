using System;
using System.Threading;
using System.Web;

namespace EjemplosFormacion.WebApi.HttpHandler
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
    public class TestAsyncHttpHandler : IHttpAsyncHandler
    {
        private HttpContext _context;

        public bool IsReusable
        {
            get
            {
                // To enable pooling, return true here.
                // This keeps the handler in memory.
                return false;
            }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            _context = context;

            _context.Response.Write("Begin IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + Environment.NewLine);

            AsynchOperation asynch = new AsynchOperation(cb, _context, extraData);
            asynch.StartAsyncWork();

            return asynch;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            _context.Response.Write("EndProcessRequest called.");
        }


        public void ProcessRequest(HttpContext context)
        {
            // This method is required but is not called.
        }

        class AsynchOperation : IAsyncResult
        {
            private bool _completed;
            private Object _state;
            private AsyncCallback _callback;
            private HttpContext _context;

            bool IAsyncResult.IsCompleted { get { return _completed; } }
            WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
            Object IAsyncResult.AsyncState { get { return _state; } }
            bool IAsyncResult.CompletedSynchronously { get { return false; } }

            public AsynchOperation(AsyncCallback callback, HttpContext context, Object state)
            {
                _callback = callback;
                _context = context;
                _state = state;
                _completed = false;
            }

            public void StartAsyncWork()
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
            }

            private void StartAsyncTask(Object workItemState)
            {
                _context.Response.Write("Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + Environment.NewLine);
        
                _context.Response.Write("Hello from Async Handler!" + Environment.NewLine);
                _completed = true;
                _callback(this);
            }
        }
    }

}