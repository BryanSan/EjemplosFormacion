using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.WebHost;

namespace EjemplosFormacion.WebApi.HostBufferPolicySelectors
{
    /// <summary>
    /// Se supone que se use para configurar si un Request (Input) o Response (Output) es Buffered o Streaming
    /// Esto quiere decir que todos los datos se cargan memoria o no respectivamente
    /// He jugado con la configuracion y no veo cambio alguno toca hondar mas en el tema
    /// </summary>
    public class NoWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        public override bool UseBufferedInputStream(object hostContext)
        {
            HttpContextBase context = hostContext as HttpContextBase;

            if (context != null)
            {
                if (string.Equals(context.Request.RequestContext.RouteData.Values["controller"]?.ToString(), "TestImage", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }

        public override bool UseBufferedOutputStream(HttpResponseMessage response)
        {
            return base.UseBufferedOutputStream(response);
        }
    }
}