using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestHttpRouteConstraints
{
    [RoutePrefix("TestIsSpecificValueHttpRouteConstraint")]
    public class TestIsSpecificValueHttpRouteConstraintController : ApiController
    {
        // Action que solo sera llamado si se cumple las condiciones que especifica el Custom Http Route Constraint
        // En este caso que el parametro id sea resuelto con un valor exacto de 1
        // A pesar de que esta Route esta registrada identicamente que la de abajo, no dara error, ya que
        // El Http Route Constraint registrado en el parametro id se encargara de diferenciarlas ya que una es llamada cuando el valor es 1 y la otra con el valor 2
        /// Recordar que Web Api debe tener uno y solo un candidato de Action al final de toda la evaluacion para llamar o dara error si consigue una ambiguedad (Varios Action que pueden ejecutar el Request)
        [Route("TestIsSpecificValueHttpRouteConstraint/{id:isSpecificValue(1)}")]
        public IHttpActionResult TestIsSpecificValueHttpRouteConstraintUno(int id)
        {
            return Ok();
        }

        // Action que solo sera llamado si se cumple las condiciones que especifica el Custom Http Route Constraint
        // En este caso que el parametro id sea resuelto con un valor exacto de 2
        // A pesar de que esta Route esta registrada identicamente que la de arriba, no dara error, ya que
        // El Http Route Constraint registrado en el parametro id se encargara de diferenciarlas ya que una es llamada cuando el valor es 1 y la otra con el valor 2
        /// Recordar que Web Api debe tener uno y solo un candidato de Action al final de toda la evaluacion para llamar o dara error si consigue una ambiguedad (Varios Action que pueden ejecutar el Request)
        [Route("TestIsSpecificValueHttpRouteConstraint/{id:isSpecificValue(2)}")]
        public IHttpActionResult TestIsSpecificValueHttpRouteConstraintDos(int id)
        {
            return Ok();
        }
    }
}
