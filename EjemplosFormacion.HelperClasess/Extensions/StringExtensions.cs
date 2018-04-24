using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace EjemplosFormacion.HelperClasess.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extension Method para validar si un string es un JsonValido
        /// </summary>
        /// <param name="source">String para validar</param>
        /// <returns></returns>
        public static bool IsValidJson(this string source)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(source))
            {
                string value = source.Trim();

                if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                    (value.StartsWith("[") && value.EndsWith("]"))) //For array
                {
                    try
                    {
                        JToken obj = JToken.Parse(value);
                        isValid = true;
                    }
                    catch (JsonReaderException)
                    {

                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Extension Method para validar que un string este un numero de veces en un string
        /// </summary>
        /// <param name="source">String a testear</param>
        /// <param name="stringToSearch">String a buscar</param>
        /// <param name="numberOfTimes">Numero de veces minimas que debe aparecer para estar correcto</param>
        /// <returns></returns>
        public static bool IsStringNumberOfTimes(this string source, string stringToSearch, int numberOfTimes)
        {
            return Regex.Matches(source, Regex.Escape(stringToSearch)).Count >= numberOfTimes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool AnyInvalidFileNameChars(this string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            return invalidFileNameChars.Intersect(fileName).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static MediaTypeHeaderValue GetMimeTypeNameFromExtension(this string ext)
        {
            var mimeNames = new Dictionary<string, string>();

            mimeNames.Add(".mp3", "audio/mpeg");    // List all supported media types; 
            mimeNames.Add(".mp4", "video/mp4");
            mimeNames.Add(".ogg", "application/ogg");
            mimeNames.Add(".ogv", "video/ogg");
            mimeNames.Add(".oga", "audio/ogg");
            mimeNames.Add(".wav", "audio/x-wav");
            mimeNames.Add(".webm", "video/webm");

            string value;

            if (mimeNames.TryGetValue(ext.ToLowerInvariant(), out value))
                return new MediaTypeHeaderValue(value);
            else
                return new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        }
    }
}
