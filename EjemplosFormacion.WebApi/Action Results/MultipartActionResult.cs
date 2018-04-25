using System;
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
    public class MultipartActionResult : IHttpActionResult
    {
        readonly MultipartContent _content;
        readonly List<MultipartItem> _multiPartItem;

        public MultipartActionResult(List<MultipartItem> multiPartItem, string subtype = "byteranges", string boundary = null)
        {
            if (multiPartItem == null || multiPartItem.Count <= 0) throw new ArgumentException("multiPartItem invalido!.");
            
            _multiPartItem = multiPartItem;

            if (boundary == null)
            {
                _content = new MultipartContent(subtype);
            }
            else
            {
                _content = new MultipartContent(subtype, boundary);
            }
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                foreach (var item in _multiPartItem)
                {
                    if (item.Stream != null)
                    {
                        var content = new StreamContent(item.Stream);

                        if (!string.IsNullOrWhiteSpace(item.ContentType))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
                        }

                        if (!string.IsNullOrWhiteSpace(item.FileName))
                        {
                            var contentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = item.FileName
                            };

                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = contentDisposition.FileName,
                                FileNameStar = contentDisposition.FileNameStar
                            };
                        }

                        _content.Add(content);
                    }
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = _content;

                return response;
            });
        }

        public class MultipartItem
        {
            public string ContentType { get; set; }

            public string FileName { get; set; }

            public Stream Stream { get; set; }
        }
    }
}