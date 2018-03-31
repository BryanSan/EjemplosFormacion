using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.AuthorizationFilters
{
    public class TestIAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
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