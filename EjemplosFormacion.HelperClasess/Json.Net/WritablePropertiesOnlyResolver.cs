using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EjemplosFormacion.HelperClasess.Json.Net
{
    /// <summary>
    /// Contract Resolver para configurar al serializador de Json.Net para que ignore las propiedades que sean de solo lectura (readonly)
    /// </summary>
    internal class WritablePropertiesOnlyResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }
    }
}
