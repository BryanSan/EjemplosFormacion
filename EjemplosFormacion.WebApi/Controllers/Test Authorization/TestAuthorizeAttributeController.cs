using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestAuthorization
{
    public class TestAuthorizeAttributeController : ApiController
    {
        // El Authorize Attribute verificara que este asignado la propiedad IPrincipal en el 
        //      Thread.CurrentPrincipal = principal; 
        //      HttpContext.Current.User = principal;
        // El supone que si esta asignada significa que el Request esta authenticado 
        [Authorize]
        public IHttpActionResult TestAuthorizeAttribute()
        {
            return Ok();
        }

        // El Authorize Attribute verificara que este asignado la propiedad IPrincipal en el 
        //      Thread.CurrentPrincipal = principal; 
        //      HttpContext.Current.User = principal;
        // El supone que si esta asignada significa que el Request esta authenticado 
        // Adicionalmente verifica que el UserName del IPrincipal asignado sea el especificado en el Attribute
        // Restrict by user:
        [Authorize(Users = "Alice,Bob")]
        public IHttpActionResult TestAuthorizeAttributeOnlySpecifiedUsers()
        {
            return Ok();
        }

        // El Authorize Attribute verificara que este asignado la propiedad IPrincipal en el 
        //      Thread.CurrentPrincipal = principal; 
        //      HttpContext.Current.User = principal;
        // El supone que si esta asignada significa que el Request esta authenticado 
        // Adicionalmente verifica que el IPrincipal asignado tenga los Roles especificados en el Attribute
        // Restrict by role:
        [Authorize(Roles = "Administrators")]
        public IHttpActionResult TestAuthorizeAttributeOnlySpecifiedRoles()
        {
            return Ok();
        }
    }
}
