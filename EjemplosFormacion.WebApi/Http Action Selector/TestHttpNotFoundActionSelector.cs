using System.Web.Http;
using System.Web.Http.Controllers;

namespace EjemplosFormacion.WebApi.HttpActionSelector
{
    /// <summary>
    /// // Custom Implementacion del servicio Web Api IHttpActionSelector para customizar como se selecciona el Action segun la Route que ha llegado
    /// </summary>
    public class TestHttpNotFoundActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

            return decriptor;
        }
    }
}