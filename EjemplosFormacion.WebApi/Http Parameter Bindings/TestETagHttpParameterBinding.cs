using EjemplosFormacion.WebApi.Stubs.Enums;
using EjemplosFormacion.WebApi.Stubs.Models;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace EjemplosFormacion.WebApi.HttpParametersBindings
{
    class TestETagHttpParameterBinding : HttpParameterBinding
    {
        TestETagMatchEnum _match;

        public TestETagHttpParameterBinding(HttpParameterDescriptor parameter, TestETagMatchEnum match)
            : base(parameter)
        {
            _match = match;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            EntityTagHeaderValue etagHeader = null;
            switch (_match)
            {
                case TestETagMatchEnum.IfNoneMatch:
                    etagHeader = actionContext.Request.Headers.IfNoneMatch.FirstOrDefault();
                    break;

                case TestETagMatchEnum.IfMatch:
                    etagHeader = actionContext.Request.Headers.IfMatch.FirstOrDefault();
                    break;
            }

            TestETagModel etag = null;
            if (etagHeader != null)
            {
                etag = new TestETagModel { Tag = etagHeader.Tag };
            }
            actionContext.ActionArguments[Descriptor.ParameterName] = etag;

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}