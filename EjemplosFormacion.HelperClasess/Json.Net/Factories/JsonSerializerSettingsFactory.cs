using EjemplosFormacion.HelperClasess.Json.Net.Abstract;
using Newtonsoft.Json;

namespace EjemplosFormacion.HelperClasess.Json.Net.Factories
{
    public class JsonSerializerSettingsFactory : IJsonSerializerSettingsFactory
    {
        public JsonSerializerSettings CreateJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore, // Serializar Nulls
                DefaultValueHandling = DefaultValueHandling.Ignore, // Serializar Default values
                ContractResolver = new WritablePropertiesOnlyResolver(), // Custom logica para serializar properties, en este caso no serializo properties que sean de solo lectura
            };
        }
    }
}