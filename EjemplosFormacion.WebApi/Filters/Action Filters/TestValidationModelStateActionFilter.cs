using EjemplosFormacion.WebApi.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    public class TestValidationModelStateActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                TestModelError error = new TestModelError(actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
        }
    }
}