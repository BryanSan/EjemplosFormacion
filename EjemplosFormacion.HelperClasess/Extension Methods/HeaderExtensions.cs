using System.Net.Http.Headers;

namespace EjemplosFormacion.HelperClasess.ExtensionMethods
{
    public static class HeaderExtensions
    {
        /// <summary>
        /// Extension method usado para extraer el start y end bytes validando que si se realizo la extraccion exitosamente
        /// </summary>
        public static bool TryReadRangeItem(this RangeItemHeaderValue range, long contentLength, out long start, out long end)
        {
            if (range.From != null)
            {
                start = range.From.Value;
                if (range.To != null)
                    end = range.To.Value;
                else
                    end = contentLength - 1;
            }
            else
            {
                end = contentLength - 1;
                if (range.To != null)
                    start = contentLength - range.To.Value;
                else
                    start = 0;
            }

            return (start < contentLength && end < contentLength);
        }
    }
}
