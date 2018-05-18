using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    class FileStreamZipRangeActionResult : IHttpActionResult
    {
        readonly List<FileStreamZipItem> _listItemsToZip;
        readonly string _fileNameZip;

        public FileStreamZipRangeActionResult(List<FileStreamZipItem> listItemsToZip, string fileNameZip = "MyZipfile")
        {
            _listItemsToZip = listItemsToZip;
            _fileNameZip = fileNameZip;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new PushStreamContent(async (outputStream, httpContext, transportContext) =>
                    {
                        using (var zipStream = new ZipOutputStream(outputStream, leaveOpen: true))
                        {
                            foreach (FileStreamZipItem itemToZip in _listItemsToZip)
                            {
                                zipStream.PutNextEntry(itemToZip.FileNameWithExtension);
                                using (Stream stream = itemToZip.File)
                                    await stream.CopyToAsync(zipStream);
                            }
                        }
                    }),
                };


                string fileNameOfZip = _fileNameZip.ToLowerInvariant().Contains(".zip") ? _fileNameZip : _fileNameZip + ".zip";
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileNameOfZip };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return result;
            });
        }

        public class FileStreamZipItem
        {
            public Stream File { get; set; }
            public string FileNameWithExtension { get; set; }
        }
    }
}