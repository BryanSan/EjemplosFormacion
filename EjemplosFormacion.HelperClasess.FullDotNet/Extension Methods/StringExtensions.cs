using System.IO;
using System.Web;

namespace EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extension Mehtod para hallar el MIME Type apartir una Extension de un archivo
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetMimeContentType(this string source)
        {
            return MimeMapping.GetMimeMapping(Path.GetExtension(source));
        }
    }
}
