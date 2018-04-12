using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.DirectRouteProviders
{
    public class TestDirectRouteProvider : DefaultDirectRouteProvider
    {
        private readonly string _centralizedPrefix;

        public TestDirectRouteProvider(string centralizedPrefix)
        {
            _centralizedPrefix = centralizedPrefix;

            if (!string.IsNullOrWhiteSpace(_centralizedPrefix))
            {
                _centralizedPrefix = _centralizedPrefix.Trim();
                if (_centralizedPrefix.Last() == '/')
                {
                    _centralizedPrefix = _centralizedPrefix.Remove(_centralizedPrefix.LastIndexOf("/"));
                }
            }
        }

        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            if (existingPrefix == null) return _centralizedPrefix;

            return string.Format("{0}/{1}", _centralizedPrefix, existingPrefix);
        }
    }
}