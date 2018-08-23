using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Custom Action Result para devolver un archivo unico entero, se devuelve un StreamContent para evitar cargarlo en memoria
    /// Sin embargo el cliente debera esperar que todo el archivo se descargue antes de empezar a usarlo
    /// Al contrario como Youtube que puedes ver el video mientras se descarga aqui deberas esperar que se descargue todo para usarlo
    /// Ideal para archivos pequeños/medianos o que no se necesiten usar mientras se estan descargando 
    /// </summary>
    class FileStreamActionResult : IHttpActionResult
    {
        readonly Stream _stream;
        readonly string _contentType;

        public FileStreamActionResult(Stream stream, string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentException("contentType vacio!.");

            _stream = stream ?? throw new ArgumentException("stream vacio!.");
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                // Devuelvo el content como StreamContent para no cargarlo en memoria
                response.Content = new StreamContent(_stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

                return response;
            }, cancellationToken);
        }
    }
}