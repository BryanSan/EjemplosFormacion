using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EjemplosFormacion.HelperClasess.ExtensionMethods
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
        /// Extension Method para saber que un FileName tiene un caracter invalido
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool AnyInvalidFileNameChars(this string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            return invalidFileNameChars.Intersect(fileName).Any();
        }


        static readonly char[] _specialChars = new char[] { ',', '\n', '\r', '"' };
        /// <summary>
        /// Extension Method para escapar caracteres invalidos en un string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EscapeInvalidCharacters(this string source)
        {
            if (source == null)
            {
                return "";
            }
            string field = source.ToString();
            if (field.IndexOfAny(_specialChars) != -1)
            {
                // Delimit the entire field with quotes and replace embedded quotes with "".
                return string.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else return field;
        }

        public static T? TryToParse<T>(this string input) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            var type = typeof(T);
            var method = type.GetMethod("TryParse", new[] { typeof(string), Type.GetType(string.Format("{0}&", type.FullName)) });
            object[] args = new object[] { input, null };
            return (bool)method.Invoke(null, args) ? (T)args[1] : default(T?);
        }

        public static bool ContainsIgnoreCase(this string input, string toMatch)
        {
            return !string.IsNullOrEmpty(toMatch) && input.IndexOf(toMatch, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
