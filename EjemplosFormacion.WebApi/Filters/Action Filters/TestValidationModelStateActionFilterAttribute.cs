using EjemplosFormacion.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter que valida el ModelState del Model Binding del Action
    /// Si tiene algun error lo pasa a una entidad y devuelve un BadRequest con dichos errores
    /// Para la validacion de Required tener cuidado con los valores default no nullable ya que no devolvera error, pasar la propiedad a nullable int -> int?
    /// </summary>
    class TestValidationModelStateActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                // Recuperamos los errores y los pasamos a una entidad
                List<string> listaErrores = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TestModelError error = new TestModelError(listaErrores);

                // Devolvemos un BadRequest con los errores recuperados
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
        }
    }
}