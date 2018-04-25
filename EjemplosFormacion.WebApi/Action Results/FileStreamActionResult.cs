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
                // Hallar el mime type segun la extension del nombre del archivo
                //var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(_tipoDeArchivo));

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

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
                        response.StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable;
                        response.Content = new StreamContent(Stream.Null);  // No content for this status.
                        response.Content.Headers.ContentRange = new ContentRangeHeaderValue(totalLenght);
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

                        return response;
                    }

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

        public void WriteToStreamRange(Stream outputStream, HttpContent content, TransportContext context, long start, long end)
        {
            try
            {
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