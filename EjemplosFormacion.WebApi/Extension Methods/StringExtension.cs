using System.IO;
using System.Web;

namespace EjemplosFormacion.WebApi.ExtensionMethods
{
    public static class StringExtension
    {
        public static string GetMimeContentType(this string source)
        {
            return MimeMapping.GetMimeMapping(Path.GetExtension(source));
        }
    }
}