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
        // Check incoming requests and modify their buffer policy
        public override bool UseBufferedInputStream(object hostContext)
        {
            HttpContextBase contextBase = hostContext as HttpContextBase;

            if (contextBase != null && contextBase.Request.ContentType != null && contextBase.Request.ContentType.Contains("multipart"))
            {
                // we are enabling streamed mode here
                return false;
            }

            // let the default behavior(buffered mode) to handle the scenario
            return base.UseBufferedInputStream(hostContext);
        }

        // You could also chnage the response behavior too...but for this example, we are not
        // going to do anything here...I overrode this method just to demonstrate the availability
        // of this method.
        public override bool UseBufferedOutputStream(System.Net.Http.HttpResponseMessage response)
        {
            return base.UseBufferedOutputStream(response);
        }
    }
}