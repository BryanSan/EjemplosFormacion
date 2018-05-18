using EjemplosFormacion.WebApi.ActionResults;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFiles
{
    // Recordar siempre configurar en el Web.Config el maximo de los archivos
    //<configuration>
    //    <system.web>
    //        <httpRuntime targetFramework = "4.7.1" maxRequestLength="2097152"/>
    //    </system.web>
    //    <system.webServer>
    //        <security>
    //            <requestFiltering>
    //                <requestLimits maxAllowedContentLength = "2147483648" />
    //            </requestFiltering>
    //        </security>
    //    </system.webServer>
    //</ configuration >
    public class TestFileController : ApiController
    {
        // ================================================================================================
        //                                          UPLOAD
        // ================================================================================================


        // Demostracion de como cargar solo UN Archivo sin Form Data (Solo el stream) en un Action del Web Api
        // Al leer todo el Stream del Request y no poder separar datos, solo te sirve para leer de 1 en 1 como un Todo
        // Esta accion solo lee el Stream del Request sin cargar todo en memoria siendo efectiva par archivos largos
        // Usar el header para saber el content type del archivo (el formato mime)
        public async Task<IHttpActionResult> TestUploadOnlyOneFileAsStreamNoAllInMemory()
        {
            // Usa el archivo para guardarlo en una bd con FileStream, o en archivos con un FileStream de .Net o lo que quieras
            // Pero aqui no cargas todo en memoria
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;

            return Ok();
        }


        // ================================================================================================
        //                                          GET
        // ================================================================================================


        // Demostracion de como devolver al cliente UN solo sin Form Data archivo usando un stream
        // Este Action devuelve el Stream del archivo entero sin streaming 
        // Devolviendo todos los bytes al cliente de manera que el cliente debe esperar que se complete la peticion para usar el Stream
        public async Task<IHttpActionResult> TestGetOnlyOneFileAsStreamNoStreaming()
        {
            // Recupera el archivo como Stream, si lo recuperas como Byte estaras matando todo lo que se quiere lograr
            // Ya que habras cargado todo en memoria, trabaja full en Stream para minimizar le uso de memoria 
            // Especialmente en archivos grandes
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;

            // Devuelve el stream del archivo que quieres devolver con el Content Type del archivo
            return new FileStreamActionResult(stream, contentType);
        }

        // Demostracion de como devolver al cliente UN archivo sin Form Data usando un stream
        // Este Action devuelve el Stream del archivo entero con streaming 
        // Devuelve solo la porcion de bytes que se piden en el header Range de manera que el cliente puede empezar a usarlos a medida que la peticion se va descargando del Stream
        public async Task<IHttpActionResult> TestGetOnlyOneFileAsStreamWithStreaming()
        {
            // Recupera el archivo como Stream, si lo recuperas como Byte estaras matando todo lo que se quiere lograr
            // Ya que habras cargado todo en memoria, trabaja full en Stream para minimizar le uso de memoria 
            // Especialmente en archivos grandes
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;
            RangeHeaderValue rangeHeader = Request.Headers.Range;

            // Devuelve el stream del archivo que quieres devolver con el Content Type del archivo junto con el header del Range que quieres devolver
            // El header Range no puede estar nulo y transmitira todo el Stream sin hacer streaming
            // Ya que le header Range es el que dice de que byte a que byte se va a leer (el chunk)
            return new FileStreamRangeActionResult(stream, contentType, rangeHeader);
        }
    }
}
