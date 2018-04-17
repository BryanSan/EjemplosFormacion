using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    /// <summary>
    /// Implement this interface to perform asynchronous authorization logic; 
    /// for example, if your authorization logic makes asynchronous I/O or network calls. 
    /// (If your authorization logic is CPU-bound, it is simpler to derive from AuthorizationFilterAttribute, because then you don't need to write an asynchronous method.)
    /// </summary>
    class TestIAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
    {

        public virtual void OnAuthorization(HttpActionContext actionContext)
        {

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "exception is flowed through the task")]
        public virtual Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            try
            {
                OnAuthorization(actionContext);
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            
            return Task.FromResult<object>(null);
        }

        Task<HttpResponseMessage> IAuthorizationFilter.ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (continuation == null)
            {
                throw new ArgumentNullException("continuation");
            }

            return ExecuteAuthorizationFilterAsyncCore(actionContext, cancellationToken, continuation);
        }

        private async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsyncCore(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            await OnAuthorizationAsync(actionContext, cancellationToken);

            if (actionContext.Response != null)
            {
                return actionContext.Response;
            }
            else
            {
                return await continuation();
            }
        }
    }
}