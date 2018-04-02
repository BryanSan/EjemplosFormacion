using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EjemplosFormacion.HelperClasess.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJson(this string stringValue)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                string value = stringValue.Trim();

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
    }
}
