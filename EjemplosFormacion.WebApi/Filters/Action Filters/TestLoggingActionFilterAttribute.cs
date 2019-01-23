using EjemplosFormacion.HelperClasess.Wrappers.Abstract;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que Loggea la ejecucion del Action, Loggea la informacion del Action, Controller, Request, Response, Excepcion y Tiempos
    /// </summary>
    class TestLoggingActionFilterAttribute : ActionFilterAttribute
    {
        [Dependency]
        public IWrapperNLog Logger { get; set; }

        private const string StopwatchKey = "StopwatchFilter.Value";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var type = actionContext.ControllerContext.Controller.GetType();
            var actionName = actionContext.ActionDescriptor.ActionName;

            Logger.Info(string.Format("{0} --> {1} - INI", type.Name, actionName));
            Logger.Debug(string.Format("{0} --> {1} - INI --> URI: {2} {3}", type.Name, actionName, request.Method, request.RequestUri));

            actionContext.Request.Properties[StopwatchKey] = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[StopwatchKey];
            var type = actionExecutedContext.ActionContext.ControllerContext.Controller.GetType();
            var response = actionExecutedContext.Response;
            var exception = actionExecutedContext.Exception;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var infoMessage = string.Format("{0} --> {1} - END --> Fin --> TimElapsed: {2} y Response: {3}", type.Name, actionName, stopwatch.Elapsed, exception != null ? "Exception" : response.StatusCode.ToString());
            var debugMessage = string.Format("{0} --> {1} - END --> Fin --> TimElapsed: {2} y Response: {3}", type.Name, actionName, stopwatch.Elapsed, exception != null ? exception.Message.ToString() : response.StatusCode.ToString());

            Logger.Info(infoMessage);
            Logger.Debug(debugMessage);
        }
    }
}