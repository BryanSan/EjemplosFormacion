using EjemplosFormacion.WebApi.ActionResults;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestFiles
{
    // Recordar siempre configurar en el Web.Config el maximo de los archivos
    //<configuration>
    //    <system.web>
    //        <httpRuntime targetFramework = "4.7.1" maxRequestLength="2097152"/>
    //    </system.web>
    //    <system.webServer>
    //        <security>
    //            <requestFiltering>
    //                <requestLimits maxAllowedContentLength = "2147483648" />
    //            </requestFiltering>
    //        </security>
    //    </system.webServer>
    //</ configuration >
    public class TestZipController : ApiController
    {
        public async Task<IHttpActionResult> TestGetZipNoStreaming()
        {
            var fileNamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/.gitignore" },
            };

            var listFiles = new List<FileStreamZipActionResult.FileStreamZipItem>();
            using (HttpClient client = new HttpClient())
            {
                foreach (var item in fileNamesAndUrls)
                {
                    using (Stream stream = await client.GetStreamAsync(item.Value))
                    {
                        var file = new FileStreamZipActionResult.FileStreamZipItem
                        {
                            File = stream,
                            FileNameWithExtension = item.Key
                        };

                        listFiles.Add(file);
                    }
                }
            }

            return new FileStreamZipActionResult(listFiles, "MyZipFile");
        }

        public async Task<IHttpActionResult> TestGetZipWithStreaming()
        {
            var fileNamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/.gitignore" },
            };

            var listFiles = new List<FileStreamZipRangeActionResult.FileStreamZipItem>();
            using (HttpClient client = new HttpClient())
            {
                foreach (var item in fileNamesAndUrls)
                {
                    using (Stream stream = await client.GetStreamAsync(item.Value))
                    {
                        var file = new FileStreamZipRangeActionResult.FileStreamZipItem
                        {
                            File = stream,
                            FileNameWithExtension = item.Key
                        };

                        listFiles.Add(file);
                    }
                }
            }

            return new FileStreamZipRangeActionResult(listFiles, "MyZipFile");
        }
    }
}
