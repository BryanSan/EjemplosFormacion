using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

namespace EjemplosFormacion.WebApi.Controllers.TestImages
{
    // https://www.strathweb.com/2012/09/dealing-with-large-files-in-asp-net-web-api/
    public class TestImageController : ApiController
    {
        public async Task<IHttpActionResult> TestUploadImage()
        {
            Stream streamOtro = await Request.Content.ReadAsStreamAsync();
            if (Request.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider streamProvider = await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());
                
                HttpContent content = streamProvider.Contents.FirstOrDefault();
                string testName = content.Headers.ContentDisposition.Name;
                Stream stream = await content.ReadAsStreamAsync();

                return Ok(stream);
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }

        public void TestUploadImage2()
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            if (Request.Content.IsMimeMultipartContent())
            {
                StreamContent content = (StreamContent)Request.Content;
                Task<Stream> task = content.ReadAsStreamAsync();
                Stream readOnlyStream = task.Result;
                Byte[] buffer = new Byte[readOnlyStream.Length];
                readOnlyStream.Read(buffer, 0, buffer.Length);
                MemoryStream memoryStream = new MemoryStream(buffer);
                Image image = Image.FromStream(memoryStream);
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }
    }
}