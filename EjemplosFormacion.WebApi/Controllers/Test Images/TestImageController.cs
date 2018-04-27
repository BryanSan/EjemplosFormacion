using EjemplosFormacion.WebApi.ActionResults;
using EjemplosFormacion.WebApi.Models;
using EjemplosFormacion.WebApi.MultipartStreamProviders;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestImages
{
    public class TestImageController : ApiController
    {
        // Demostracion de como cargar Imagenes junto con Form Data en un Action del Web Api
        // El problema es que no tienes manera de saber cual es File y cual es Form Data de manera facil y te toca guiarte del orden que fueron agregados
        // Mejor usar la implementacion del InMemoryMultipartFormDataStreamProvider que hace el trabajo de separar el Form Data y File en colecciones separadas y que sea mas comodo
        // Puedes subir una coleccion de imagenes y datos, ya que esto te lo permite el Multipart
        // Esta accion carga todo en Memoria siendo no efectiva para archivos largos
        public async Task<IHttpActionResult> TestUploadMultipartAllInMemory()
        {
            // Si tiene el Content-Type de multipart/form-data
            if (Request.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider());

                Stream stream = await provider.Contents[0].ReadAsStreamAsync();
                string @string = await provider.Contents[1].ReadAsStringAsync();

                return Ok();
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }

        // Demostracion de como cargar Imagenes junto con Form Data en un Action del Web Api, separa los archivos de los files mediante sus propiedades
        // Puedes subir una coleccion de imagenes y datos, ya que esto te lo permite el Multipart
        // Esta implementacion separa los Files y Form Data en colecciones separadas para un comodo tratamiento
        // Esta accion carga todo en memoria siendo no efectiva para archivos largos
        public async Task<IHttpActionResult> TestUploadCustomMultipartAllInMemory()
        {
            // Si tiene el Content-Type de multipart/form-data
            if (Request.Content.IsMimeMultipartContent())
            {
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                var provider = await content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

                //access form data
                NameValueCollection formData = provider.FormData;

                //access files
                IList<HttpContent> files = provider.Files;

                //Example: reading a file's stream like below
                HttpContent file1 = files[0];
                Stream stream = await file1.ReadAsStreamAsync();

                return Ok();
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }

        // Demostracion de como cargar solo Imagenes sin Form Data en un Action del Web Api
        // Al leer todo el Stream del Request y no poder separar datos, solo te sirve para leer de 1 en 1 como un Todo
        // Esta accion solo lee el Stream del Request sin cargar todo en memoria siendo efectiva par archivos largos
        // Usar el header para saber el content type del archivo (el formato mime)
        public async Task<IHttpActionResult> TestUploadOnlyImagesAsStreamNoAllInMemory()
        {
            // Usa el archivo para guardarlo en una bd con FileStream, o en archivos con un FileStream de .Net o lo que quieras
            // Pero aqui no cargas todo en memoria
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;

            return Ok();
        }

        // Demostracion de como devolver al cliente un archivo usando un stream
        // Este Action devuelve el Stream del archivo entero sin streaming 
        // Devolviendo todos los bytes al cliente de manera que el cliente debe esperar que se complete la peticion para usar el Stream
        public async Task<IHttpActionResult> TestGetOnlyImagesAsStreamNoStreaming()
        {
            // Recupera el archivo como Stream, si lo recuperas como Byte estaras matando todo lo que se quiere lograr
            // Ya que habras cargado todo en memoria, trabaja full en Stream para minimizar le uso de memoria 
            // Especialmente en archivos grandes
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;

            // Devuelve el stream del archivo que quieres devolver con el Content Type del archivo
            return new FileStreamActionResult(stream, contentType);
        }

        // Demostracion de como devolver al cliente un archivo usando un stream
        // Este Action devuelve el Stream del archivo entero con streaming 
        // Devuelve solo la porcion de bytes que se piden en el header Range de manera que el cliente puede empezar a usarlos a medida que la peticion se va descargando del Stream
        public async Task<IHttpActionResult> TestGetOnlyImagesAsStreamWithStreaming()
        {
            // Recupera el archivo como Stream, si lo recuperas como Byte estaras matando todo lo que se quiere lograr
            // Ya que habras cargado todo en memoria, trabaja full en Stream para minimizar le uso de memoria 
            // Especialmente en archivos grandes
            Stream stream = await Request.Content.ReadAsStreamAsync();
            string contentType = Request.Content.Headers.ContentType.MediaType;

            // Devuelve el stream del archivo que quieres devolver con el Content Type del archivo junto con el header del Range que quieres devolver
            // El header Range no puede estar nulo y transmitira todo el Stream sin hacer streaming
            // Ya que le header Range es el que dice de que byte a que byte se va a leer (el chunk)
            return new FileStreamActionResult(stream, contentType, Request.Headers.Range);
        }

        // Extraido de -> https://stackoverflow.com/questions/38069730/how-to-create-a-multipart-http-response-with-asp-net-core
        // Demostracion de como devolver al cliente varios archivos y la posibilidad de devolver FormData usando un MultiPartResult
        // Esta Action devuelve un MultiPartResult entero sin Streaming
        // El navegador Firefox descargara los archivos separados con su Content-Type y Filename, mientras que los demas navegadores descargaran un unico archivo sin formato
        // En caso del cliente puede ser una aplicacion que lea el response con el metodo ReadAsMultiPartAsync() y tratar los Files y Form Data separadamente
        // Para habilitar el ReadAsMultiPartAsync() en el Cliente, agregar el Assembly necesario o el Nuget de Formatting.Extensions
        // Aprovechar Custom la implementacion del MemoryStreamProvider "InMemoryMultipartFormDataStreamProvider" para separas los Files y Form Data y que sea mas comodo 
        public IHttpActionResult TestGetMultipartFileAsStreamNoStreaming()
        {
            // Recupera el archivo como Stream, si lo recuperas como Byte estaras matando todo lo que se quiere lograr
            // Ya que habras cargado todo en memoria, trabaja full en Stream para minimizar le uso de memoria 
            // Especialmente en archivos grandes
            FileStream stream = File.Open(@"C:\Users\bryan.sanchez\Documents\Visual Studio 2017\1.jpeg", FileMode.Open);
            FileStream stream2 = File.Open(@"C:\Users\bryan.sanchez\Documents\Visual Studio 2017\2.jpeg", FileMode.Open);

            // Creas la lista de archivos que quieres devolver, con su Stream, Content Type y demas
            var multipartContents = new List<MultipartActionResult.MultipartFileItem>
            {
                new MultipartActionResult.MultipartFileItem()
                {
                    ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(stream.Name)),
                    FileName = "1.jpeg",
                    Stream = stream
                },
                new MultipartActionResult.MultipartFileItem()
                {
                    ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(stream2.Name)),
                    FileName = "2.jpeg",
                    Stream = stream2
                }
            };

            var multiPartFormDataItem = new MultipartActionResult.MultipartFormDataItem()
            {
                ParameterName = "parameter",
                ObjectData = new TestModel
                {
                    Edad = 22,
                    Nombre ="pepito"
                }
            };

            // Pasas la lista al Action Result que la tratara y creara el Content
            return new MultipartActionResult(multiPartFormDataItem, multipartContents);
        }
    }
}