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
        // Demostracion de como cargar solo Imagenes y no Form Data en un Action del Web Api
        // Puedes subir una coleccion de imagenes, ya que esto te lo permite el Multipart
        // NOTA -> TAMBIEN PUEDES CARGAR FORM DATA, PERO CON EL CUSTOM MultipartProvider DE MAS ABAJO SE SEPARA MEJOR LOS Files DE LA Form Data
        // Esta accion carga todo en Memoria siendo no efectiva para archivos largos
        public async Task<IHttpActionResult> TestUploadOnlyImagesAsMultipartAllInMemory()
        {
            // Si tiene el Content-Type de multipart/form-data
            if (Request.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider());

                Stream stream = await provider.Contents[0].ReadAsStreamAsync();

                return Ok();
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }

        // Demostracion de como cargar Imagenes junto con Form Data en un Action del Web Api, separa los archivos de los files mediante sus propiedades
        // Puedes subir una coleccion de imagenes y datos, ya que esto te lo permite el Multipart
        // Esta accion carga todo en memoria siendo no efectiva para archivos largos
        public async Task<IHttpActionResult> TestUploadImagesAndDataAsMultipartAllInMemory()
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
        public async Task<IHttpActionResult> TestUploadOnlyImagesNoInMemory()
        {
            // Usa el archivo para guardarlo en una bd con FileStream, o en archivos con un FileStream de .Net o lo que quieras
            // Pero aqui no cargas todo en memoria
            Stream stream = await Request.Content.ReadAsStreamAsync();

            return Ok();
        }
    }
}