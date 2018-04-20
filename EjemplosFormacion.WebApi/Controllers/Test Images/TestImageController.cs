using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.Test_Images
{
    public class TestImageController : ApiController
    {
        public async Task<IHttpActionResult> Post()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider streamProvider = await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());

                foreach (HttpContent content in streamProvider.Contents)
                {
                    Stream stream = content.ReadAsStreamAsync().Result;
                    Image image = Image.FromStream(stream);

                    string testName = content.Headers.ContentDisposition.Name;
                }

                return Ok();
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }
    }
}
