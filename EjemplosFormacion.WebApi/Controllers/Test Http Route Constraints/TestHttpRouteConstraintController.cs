using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.HttpTestRouteConstraints
{
    // https://docs.microsoft.com/es-es/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
    [RoutePrefix("TestHttpRouteConstraint")]
    public class TestHttpRouteConstraintController : ApiController
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

        // Action que solo sera llamado si se cumple las condiciones que especifica el Custom Http Route Constraint
        // En este caso que el parametro id sea resuelto con un valor distinto de 0
        [Route("TestNonZeroHttpRouteConstraint/{id:nonZero}")]
        public IHttpActionResult TestNonZeroHttpRouteConstraint(int id)
        {
            return Ok();
        }

        // Matches uppercase or lowercase Latin alphabet characters (a-z, A-Z)
        [Route("TestAlphaHttpRouteConstraint/{id:alpha}")]
        public IHttpActionResult TestAlphaHttpRouteConstraint(string id)
        {
            return Ok();
        }

        // Matches a Boolean value
        [Route("TestBoolHttpRouteConstraint/{id:bool}")]
        public IHttpActionResult TestBoolHttpRouteConstraint(bool id)
        {
            return Ok();
        }

        // Matches a DateTime value
        [Route("TestDateTimeHttpRouteConstraint/{id:datetime}")]
        public IHttpActionResult TestDateTimeHttpRouteConstraint(DateTime id)
        {
            return Ok();
        }

        // Matches a decimal value
        [Route("TestDecimalHttpRouteConstraint/{id:decimal}")]
        public IHttpActionResult TestDecimalHttpRouteConstraint(decimal id)
        {
            return Ok();
        }

        // Matches a 64-bit floating-point value
        [Route("TestDoubleHttpRouteConstraint/{id:double}")]
        public IHttpActionResult TestDoubleHttpRouteConstraint(double id)
        {
            return Ok();
        }

        // Matches a 32-bit floating-point value
        [Route("TestFloatHttpRouteConstraint/{id:float}")]
        public IHttpActionResult TestFloatHttpRouteConstraint(float id)
        {
            return Ok();
        }

        // Matches a GUID value
        [Route("TestGuidtHttpRouteConstraint/{id:guid}")]
        public IHttpActionResult TestGuidtHttpRouteConstraint(Guid id)
        {
            return Ok();
        }

        // Matches a 32-bit integer value
        [Route("TestIntHttpRouteConstraint/{id:int}")]
        public IHttpActionResult TestIntHttpRouteConstraint(int id)
        {
            return Ok();
        }

        // Matches a string with the specified length
        [Route("TestLenghtHttpRouteConstraint/{id:length(6)}")]
        public IHttpActionResult TestLenghtHttpRouteConstraint(string id)
        {
            return Ok();
        }

        // Matches a string with the specified range of lengths
        [Route("TestRangeLenghtHttpRouteConstraint/{id:length(10,15)}")]
        public IHttpActionResult TestRangeLenghtHttpRouteConstraint(string id)
        {
            return Ok();
        }

        // Matches a 64-bit integer value
        [Route("TestLongHttpRouteConstraint/{id:long}")]
        public IHttpActionResult TestLongHttpRouteConstraint(long id)
        {
            return Ok();
        }

        // Matches an integer with a maximum value
        [Route("TestMaxHttpRouteConstraint/{id:max(10)}")]
        public IHttpActionResult TestMaxHttpRouteConstraint(int id)
        {
            return Ok();
        }

        // Matches a string with a maximum length
        [Route("TestMaxLengthHttpRouteConstraint/{id:maxlength(10)}")]
        public IHttpActionResult TestMaxLengthHttpRouteConstraint(string id)
        {
            return Ok();
        }

        // Matches an integer with a minimum value.
        [Route("TestMinHttpRouteConstraint/{id:min(10)}")]
        public IHttpActionResult TestMinHttpRouteConstraint(int id)
        {
            return Ok();
        }

        // Matches a string with a minimum length.
        [Route("TestMinLengthHttpRouteConstraint/{id:minlength(10)}")]
        public IHttpActionResult TestMinLengthHttpRouteConstraint(string id)
        {
            return Ok();
        }

        // Matches an integer within a range of values.
        [Route("TestRangeHttpRouteConstraint/{id:range(1,10)}")]
        public IHttpActionResult TestRangeHttpRouteConstraint(int id)
        {
            return Ok();
        }

        // Matches a regular expression.
        [Route(@"TestRegexHttpRouteConstraint/{id:regex(^\d{3}-\d{3}-\d{4}$)}")]
        public IHttpActionResult TestRegexHttpRouteConstraint(string id)
        {
            return Ok();
        }

    }
}