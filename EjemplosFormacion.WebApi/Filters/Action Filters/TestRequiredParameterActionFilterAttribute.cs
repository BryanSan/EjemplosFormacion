using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.ActionFilters
{
    /// <summary>
    /// Action Filter usado para validar que los parametros marcados como [Required] existan, no esten nulos ni esten vacios (para las listas)
    /// Marca los parametros del Action que necesitas obligatoriamente con el atributo [Required]
    /// Cuidado que esto solo valida los parametros del Action ya que las propiedades marcadas como Required en el Dto o entidad recibida se validaran con el Model Binding y Model Validation
    /// </summary>
    public class TestRequiredParameterActionFilterAttribute : ActionFilterAttribute
    {
        // Almacena los Required Parameters que ya han sido evaluados en Request anteriores
        private readonly ConcurrentDictionary<Tuple<HttpMethod, string>, List<string>> _Cache = new ConcurrentDictionary<Tuple<HttpMethod, string>, List<string>>();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Get the request's required parameters.
            List<string> requiredParameters = GetRequiredParameters(actionContext);

            // If the required parameters are valid then continue with the request.
            // Otherwise, return status code 400.
            if (ValidateParameters(actionContext, requiredParameters))
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                string errorMessage = string.Format("Debe especificar los siguientes parámetros : {0}", string.Join(",", requiredParameters));
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }
        }

        // Valida que los parametros del Action existan, no sean nulos o vacios (para las listas)
        private static bool ValidateParameters(HttpActionContext actionContext, List<string> requiredParameters)
        {
            // If the list of required parameters is null or containst no parameters 
            // then there is nothing to validate.  
            // Return true.
            if (requiredParameters == null || requiredParameters.Count == 0)
            {
                return true;
            }

            // Attempt to find at least one required parameter that is null.
            bool hasNullParameter =
                actionContext
                .ActionArguments
                .Any(a => requiredParameters.Contains(a.Key) && a.Value == null);

            // If a null required paramater was found then return false.  
            if (hasNullParameter)
            {
                return false;
            }

            // Attemp to find at least one required parameter that is a list and is empty
            bool hasEmptyCollecionParameter =
                actionContext
                .ActionArguments
                .Where(a => requiredParameters.Contains(a.Key) && a.Value is IEnumerable<object>)
                .Select(a => a.Value as IEnumerable<object>)
                .Any(a => a.Count() <= 0);

            // If a required paramater that is a List and is empty was found then return false.  
            return !hasEmptyCollecionParameter;
        }

        // Obtiene la lista de parametros para un Action, diferenciandolos por el Request Uri y el HttpMethod usado
        private List<string> GetRequiredParameters(HttpActionContext actionContext)
        {
            // Instantiate a list of strings to store the required parameters.
            List<string> result = null;

            // Instantiate a tuple using the request's http method and the local path.
            // This will be used to add/lookup the required parameters in the cache.
            Tuple<HttpMethod, string> request =
                new Tuple<HttpMethod, string>(
                    actionContext.Request.Method,
                    actionContext.Request.RequestUri.LocalPath);

            // Attempt to find the required parameters in the cache.
            if (!_Cache.TryGetValue(request, out result))
            {
                // If the required parameters were not found in the cache then get all
                // parameters decorated with the 'RequiredAttribute' from the action context.
                result =
                    actionContext
                    .ActionDescriptor
                    .GetParameters()
                    .Where(p => p.GetCustomAttributes<RequiredAttribute>().Any())
                    .Select(p => p.ParameterName)
                    .ToList();

                // Add the required parameters to the cache.
                _Cache.TryAdd(request, result);
            }

            // Return the required parameters.
            return result;
        }
    }
}