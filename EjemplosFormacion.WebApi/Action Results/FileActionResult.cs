using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    class FileActionResult : IHttpActionResult
    {
        readonly Stream _stream;
        readonly string _contentType;

        public FileActionResult(Stream memoryStream, string contentType)
        {
            _stream = memoryStream;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(_stream)
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

                return response;
            }, cancellationToken);
        }
    }
}