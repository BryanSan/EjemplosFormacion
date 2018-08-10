using System.IO;
using System.Web;

namespace EjemplosFormacion.HelperClasess.FullDotNet.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string GetMimeContentType(this string source)
        {
            return MimeMapping.GetMimeMapping(Path.GetExtension(source));
        }
    }
}
