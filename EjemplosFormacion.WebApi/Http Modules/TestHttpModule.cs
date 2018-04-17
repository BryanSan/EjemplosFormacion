using System;
using System.Web;

namespace EjemplosFormacion.WebApi.HttpModules
{
    /// <summary>
    /// HttpModule es codigo que se inicia al principio y al final de todo el Pipeline de Asp.Net (Por lo tanto de la ejecucion del codigo del Web Api) para un Request
    /// Lo registras en el Web.Config, dandole un nombre y referenciandolo con su Full Qualified Name
    ///  <system.webServer>
    ///    <modules>
    ///      <add name = "TestHttpModule" type="EjemplosFormacion.WebApi.HttpModules.TestHttpModule" />
    ///    </modules>
    ///  </system.webServer>
    ///  
    /// El Orden de Trigger Eventos en el Pipeline es ->
    /// 
    /// BeginRequest -> The BeginRequest event signals the creation of any given new request. This event is always raised and is always the first event to occur during the processing of a request.
    /// AuthenticateRequest -> The AuthenticateRequest event signals that the configured authentication mechanism has authenticated the current request. Subscribing to the AuthenticateRequest event ensures that the request will be authenticated before processing the attached module or event handler.
    /// PostAuthenticateRequest -> The PostAuthenticateRequest event is raised after the AuthenticateRequest event has occurred. Functionality that subscribes to the PostAuthenticateRequest event can access any data that is processed by the PostAuthenticateRequest.
    /// AuthorizeRequest -> The AuthorizeRequest event signals that ASP.NET has authorized the current request. Subscribing to the AuthorizeRequest event ensures that the request will be authenticated and authorized before processing the attached module or event handler. 
    /// PostAuthorizeRequest -> The PostAuthorizeRequest event signals that ASP.NET has authorized the current request. Subscribing to the PostAuthorizeRequest event ensures authentication and authorization of the request before processing the attached module or event handler. 
    /// ResolveRequestCache -> Occurs when ASP.NET finishes an authorization event to let the caching modules serve requests from the cache, bypassing execution of the event handler (for example, a page or an XML Web service).
    /// PostResolveRequestCache -> Occurs when ASP.NET bypasses execution of the current event handler and allows a caching module to serve a request from the cache.
    /// MapRequestHandler -> The MapRequestHandler event is used by the ASP.NET infrastructure to determine the request handler for the current request (This API supports the product infrastructure and is not intended to be used directly from your code)
    /// PostMapRequestHandler -> Occurs when ASP.NET has mapped the current request to the appropriate event handler. 
    /// AcquireRequestState -> Occurs when ASP.NET acquires the current state (for example, session state) that is associated with the current request.
    /// PostAcquireRequestState -> Occurs when the request state (for example, session state) that is associated with the current request has been obtained.
    /// PreRequestHandlerExecute -> Occurs just before ASP.NET starts executing an event handler (for example, a page or an XML Web service).
    ///             CODE EXECUTED (Message Handlers, Filters, Actions)
    /// PostRequestHandlerExecute -> Occurs when the ASP.NET event handler (for example, a page or an XML Web service) finishes execution.
    /// ReleaseRequestState -> When the ReleaseRequestState event is raised, the application is finished with the request and ASP.NET is signaled to store the request state.
    /// PostReleaseRequestState -> Occurs when ASP.NET has completed executing all request event handlers and the request state data has been stored.
    /// UpdateRequestCache -> Occurs when ASP.NET finishes executing an event handler in order to let caching modules store responses that will be used to serve subsequent requests from the cache.
    /// PostUpdateRequestCache -> Occurs when ASP.NET finishes updating caching modules and storing responses that are used to serve subsequent requests from the cache.
    /// LogRequest -> Occurs just before ASP.NET performs any logging for the current request. The LogRequest event is raised even if an error occurs. You can provide an event handler for the LogRequest event to provide custom logging for the request.
    /// PostLogRequest -> Occurs when ASP.NET has completed processing all the event handlers for the LogRequest event.
    /// EndRequest -> Occurs as the last event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
    /// PreSendRequestHeaders -> Occurs just before ASP.NET sends HTTP headers to the client.
    /// PreSendRequestContent -> Occurs just before ASP.NET sends content to the client. The PreSendRequestContent event may occur multiple times.
    /// </summary>
    class TestHttpModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;

            context.AuthenticateRequest += Context_AuthenticateRequest;
            context.PostAuthenticateRequest += Context_PostAuthenticateRequest;

            context.AuthorizeRequest += Context_AuthorizeRequest;
            context.PostAuthorizeRequest += Context_PostAuthorizeRequest;

            context.ResolveRequestCache += Context_ResolveRequestCache;
            context.PostResolveRequestCache += Context_PostResolveRequestCache;

            context.MapRequestHandler += Context_MapRequestHandler;
            context.PostMapRequestHandler += Context_PostMapRequestHandler;

            context.AcquireRequestState += Context_AcquireRequestState;
            context.PostAcquireRequestState += Context_PostAcquireRequestState;

            context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
            // CODE EXECUTED(Message Handlers, Filters, Actions)
            context.PostRequestHandlerExecute += Context_PostRequestHandlerExecute;

            context.ReleaseRequestState += Context_ReleaseRequestState;
            context.PostReleaseRequestState += Context_PostReleaseRequestState;
            
            context.UpdateRequestCache += Context_UpdateRequestCache;
            context.PostUpdateRequestCache += Context_PostUpdateRequestCache;

            context.LogRequest += Context_LogRequest;
            context.PostLogRequest += Context_PostLogRequest;

            context.EndRequest += Context_EndRequest;

            context.PreSendRequestHeaders += Context_PreSendRequestHeaders;
            context.PreSendRequestContent += Context_PreSendRequestContent;
        }
       

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            // The BeginRequest event signals the creation of any given new request.This event is always raised and is always the first event to occur during the processing of a request.
        }



        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            // The AuthenticateRequest event signals that the configured authentication mechanism has authenticated the current request.Subscribing to the AuthenticateRequest event ensures that the request will be authenticated before processing the attached module or event handler.
        }

        private void Context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            // The PostAuthenticateRequest event is raised after the AuthenticateRequest event has occurred. Functionality that subscribes to the PostAuthenticateRequest event can access any data that is processed by the PostAuthenticateRequest.
        }



        private void Context_AuthorizeRequest(object sender, EventArgs e)
        {
            // The AuthorizeRequest event signals that ASP.NET has authorized the current request. Subscribing to the AuthorizeRequest event ensures that the request will be authenticated and authorized before processing the attached module or event handler. 
        }

        private void Context_PostAuthorizeRequest(object sender, EventArgs e)
        {
            // The PostAuthorizeRequest event signals that ASP.NET has authorized the current request. Subscribing to the PostAuthorizeRequest event ensures authentication and authorization of the request before processing the attached module or event handler. 
        }



        private void Context_ResolveRequestCache(object sender, EventArgs e)
        {
            // Occurs when ASP.NET finishes an authorization event to let the caching modules serve requests from the cache, bypassing execution of the event handler (for example, a page or an XML Web service).
        }

        private void Context_PostResolveRequestCache(object sender, EventArgs e)
        {
            // Occurs when ASP.NET bypasses execution of the current event handler and allows a caching module to serve a request from the cache.
        }



        private void Context_MapRequestHandler(object sender, EventArgs e)
        {
            // The MapRequestHandler event is used by the ASP.NET infrastructure to determine the request handler for the current request (This API supports the product infrastructure and is not intended to be used directly from your code)
        }

        private void Context_PostMapRequestHandler(object sender, EventArgs e)
        {
            // Occurs when ASP.NET has mapped the current request to the appropriate event handler. 
        }



        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            // Occurs when ASP.NET acquires the current state (for example, session state) that is associated with the current request.
        }

        private void Context_PostAcquireRequestState(object sender, EventArgs e)
        {
            // Occurs when the request state (for example, session state) that is associated with the current request has been obtained.
        }



        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // Occurs just before ASP.NET starts executing an event handler (for example, a page or an XML Web service).
        }

        // ===========================================================================
        //              CODE EXECUTED (Message Handlers, Filters, Actions)
        //============================================================================

        private void Context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            // Occurs when the ASP.NET event handler (for example, a page or an XML Web service) finishes execution.
        }



        private void Context_ReleaseRequestState(object sender, EventArgs e)
        {
            // When the ReleaseRequestState event is raised, the application is finished with the request and ASP.NET is signaled to store the request state.
        }

        private void Context_PostReleaseRequestState(object sender, EventArgs e)
        {
            // Occurs when ASP.NET has completed executing all request event handlers and the request state data has been stored.
        }



        private void Context_UpdateRequestCache(object sender, EventArgs e)
        {
            // Occurs when ASP.NET finishes executing an event handler in order to let caching modules store responses that will be used to serve subsequent requests from the cache.
        }

        private void Context_PostUpdateRequestCache(object sender, EventArgs e)
        {
            // Occurs when ASP.NET finishes updating caching modules and storing responses that are used to serve subsequent requests from the cache.
        }



        private void Context_LogRequest(object sender, EventArgs e)
        {
            // Occurs just before ASP.NET performs any logging for the current request. The LogRequest event is raised even if an error occurs. You can provide an event handler for the LogRequest event to provide custom logging for the request.
        }

        private void Context_PostLogRequest(object sender, EventArgs e)
        {
            // Occurs when ASP.NET has completed processing all the event handlers for the LogRequest event.
        }




        private void Context_EndRequest(object sender, EventArgs e)
        {
            // Occurs as the last event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        }



        private void Context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // Occurs just before ASP.NET sends HTTP headers to the client.
        }




        private void Context_PreSendRequestContent(object sender, EventArgs e)
        {
            // Occurs just before ASP.NET sends content to the client. The PreSendRequestContent event may occur multiple times.
        }
    }
}