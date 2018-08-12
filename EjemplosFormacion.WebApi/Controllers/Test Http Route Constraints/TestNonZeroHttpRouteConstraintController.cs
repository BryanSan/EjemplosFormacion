using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpRouteConstraints
{
    [RoutePrefix("TestNonZeroHttpRouteConstraint")]
    public class TestNonZeroHttpRouteConstraintController : ApiController
    {
        // Action que solo sera llamado si se cumple las condiciones que especifica el Custom Http Route Constraint
        // En este caso que el parametro id sea resuelto con un valor distinto de 0
        [Route("TestNonZeroHttpRouteConstraint/{id:nonZero}")]
        public IHttpActionResult TestNonZeroHttpRouteConstraint(int id)
        {
            return Ok();
        }
    }
}
