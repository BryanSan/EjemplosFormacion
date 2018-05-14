using EjemplosFormacion.HelperClasess.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Custom Action Result para devolver un archivo unico, sea entero o como Partial Content usando la clase PushStreamContent como Content para Streaming
    /// Para marcar que un archivo se debe devolver como Partial Content, se debe pasar en el constructor el Header RangeHeaderValue con el rango de bytes (inicio y fin) a devolver
    /// Esto para saber que de 1000 bytes voy a devolver del 1 al 10 luego del 10 al 20 y asi (Streaming)
    /// Ideal para archivos largos o para Videos los cuales necesitan reproducirse a medida que se van descargando (Como Youtube
    /// </summary>
    class FileStreamActionResult : IHttpActionResult
    {
        readonly Stream _stream;
        readonly string _contentType;
        readonly RangeHeaderValue _rangeHeader;

        public FileStreamActionResult(Stream stream, string contentType, RangeHeaderValue rangeHeader = null)
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentException("contentType vacio!.");

            _stream = stream ?? throw new ArgumentException("stream vacio!.");
            _contentType = contentType;
            _rangeHeader = rangeHeader;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                // Si no tengo Range Header significa que debo devolver todos los bytes
                if (_rangeHeader == null || !_rangeHeader.Ranges.Any())
                {
                    response.Content = new StreamContent(_stream);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

                    return response;
                }
                else
                {
                    long start = 0, end = 0, totalLenght;

                    totalLenght = _stream.Length;

                    // 1. If the unit is not 'bytes'.
                    // 2. If there are multiple ranges in header value.
                    // 3. If start or end position is greater than file length.
                    if (_rangeHeader.Unit != "bytes" || _rangeHeader.Ranges.Count > 1 || _rangeHeader.Ranges.First().TryReadRangeItem(totalLenght, out start, out end))
                    {
                        // Devolvemos error que no se puede satisfaces el Range pedido
                        response.StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable;
                        response.Content = new StreamContent(Stream.Null);  // No content for this status.
                        response.Content.Headers.ContentRange = new ContentRangeHeaderValue(totalLenght);
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

                        return response;
                    }

                    // Creo el header que marca el inicio, fin y total lenght del Stream a pasar
                    var contentRange = new ContentRangeHeaderValue(start, end, totalLenght);

                    // We are now ready to produce partial content.
                    response.StatusCode = HttpStatusCode.PartialContent;
                    response.Content = new PushStreamContent((outputStream, httpContent, transpContext)
                    =>
                    {
                        WriteToStreamRange(outputStream, httpContent, transpContext, start, end);
                    }, _contentType);

                    response.Content.Headers.ContentLength = end - start + 1;
                    response.Content.Headers.ContentRange = contentRange;

                    return response;
                }
            }, cancellationToken);
        }

        // Escribimos en el Stream de salida el rango de bytes solicitado de mi Source Stream
        private void WriteToStreamRange(Stream outputStream, HttpContent content, TransportContext context, long start, long end)
        {
            try
            {
                // Extension method usado para escribir en un stream el rango de bytes solicitado de mi Source Stream
                _stream.WritePartialContentToOutputStream(outputStream, start, end);
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}