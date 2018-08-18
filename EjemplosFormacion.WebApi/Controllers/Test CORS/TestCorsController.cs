using EjemplosFormacion.WebApi.CORSPolices;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EjemplosFormacion.WebApi.Controllers.TestCORS
{
    // Habilita las Cross Origin Request, esto es hacer una peticion desde un navegador a otro dominio distinto al que esta alojado el Web Api
    // Esto es:
    //      http://example.net - Different domain
    //      http://example.com:9000/foo.html - Different port
    //      https://example.com/foo.html - Different scheme
    //      http://www.example.com/foo.html - Different subdomain
    // The origins parameter of the [EnableCors] attribute specifies which origins are allowed to access the resource. The value is a comma-separated list of the allowed origins. To allow all methods, use the wildcard value "*".
    // The headers parameter of the [EnableCors] attribute specifies which author request headers are allowed. To allow any headers, set headers to "*". Set headers to a comma-separated list of the allowed headers:
    // The methods parameter of the [EnableCors] attribute specifies which HTTP methods are allowed to access the resource.  The value is a comma-separated list of the allowed HTTP methods. To allow all methods, use the wildcard value "*".
    // If you set headers to anything other than "*", you should include at least "accept", "content-type", and "origin", plus any custom headers that you want to support.
    // By default, the browser does not expose all of the response headers to the application. The response headers that are available by default are:
    //      Cache-Control
    //      Content-Language
    //      Content-Type
    //      Expires
    //      Last-Modified
    //      Pragma
    // The CORS spec calls these simple response headers.To make other headers available to the application, set the exposedHeaders parameter of[EnableCors].
    // Credentials require special handling in a CORS request. By default, the browser does not send any credentials with a cross-origin request. Credentials include cookies as well as HTTP authentication schemes. To send credentials with a cross-origin request, the client must set XMLHttpRequest.withCredentials to true.
    // In addition, the server must allow the credentials. To allow cross-origin credentials in Web API, set the SupportsCredentials property to true on the [EnableCors] attribute
    // If this property is true, the HTTP response will include an Access-Control-Allow-Credentials header. This header tells the browser that the server allows credentials for a cross-origin request.
    // The CORS spec also states that setting origins to "*" is invalid if SupportsCredentials is true.
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
    //      EnableCors configurado completo y cerrado al nivel de Controller
    //[EnableCors(origins: "http://www.contoso.com , http://www.example.com", headers: "accept , content-type ", methods: "GET , POST", exposedHeaders: "X-Custom-Header", SupportsCredentials = true)]
    //      EnableCors configurado completo y abierto al nivel de Controller
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    // Si quieres deshabilitar el CORS para un Controller o Action en especifico simplemente adornalo con el DisableCors Attribute
    // [DisableCors]
    public class TestCorsController : ApiController
    {
        //      EnableCors configurado completo y abierto al nivel de Action
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EnableCorsPerAction()
        {
            return Ok();
        }

        // Si quieres deshabilitar el CORS para un Controller o Action en especifico simplemente adornalo con el DisableCors Attribute
        [DisableCors]
        public IHttpActionResult DisableCorsPerAction()
        {
            return Ok();
        }

        [TestCorsPolicy]
        public IHttpActionResult EnableCorsPerActionWithCustomPolicy()
        {
            return Ok();
        }
    }
}
