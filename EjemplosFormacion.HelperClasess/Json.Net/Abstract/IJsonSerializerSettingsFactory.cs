using Newtonsoft.Json;

namespace EjemplosFormacion.HelperClasess.Json.Net.Abstract
{
    public interface IJsonSerializerSettingsFactory
    {
        JsonSerializerSettings CreateJsonSerializerSettings();
    }
}
