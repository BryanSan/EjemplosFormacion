using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace EjemplosFormacion.WebApi.ModelBinders
{
    /// <summary>
    /// A model binder gets raw input values from a value provider. This design separates two distinct functions:
    /// The value provider takes the HTTP request and populates a dictionary of key-value pairs.
    /// The model binder uses this dictionary to populate the model.
    /// Custom Model Binder para rellenar un parametro Complex Type de un Action de un Controller con una custom logica
    /// Como en este caso que se tiene un parametro con dos valores en el Query String separado por , que seran usado para llenar dos propiedades del Complext Type
    /// Para registrar el Model Binder para un tipo puedes
    ///     Colocarle al tipo que quieres soportar el Attribute ->
    ///         [ModelBinder(typeof(TestModelBinder))]
    ///     Colocar en el Action del Controller un Attribute junto con el Model Binder a usar ->
    ///         [ModelBinder(typeof(TestModelBinder))] TestModelBinder.GeoPoint location
    ///     Registrar el Model Binder en los Servicios del Web Api, insertandolo de PRIMERO ya que si lo insertas luego del Default, nunca sera llamado ya que el Default Model Binder ataja a todos los Types
    ///     Y especificar en el Action del Controller que ese parametro sera llenado por un Model Binder o Value Provider ->
    ///                     En el WebApiConfig.cs ->
    ///                         var provider = new SimpleModelBinderProvider(typeof(TestModelBinder.GeoPoint), new TestModelBinder());
    ///                         config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
    ///                         
    ///                         // Nueva manera de crear un ModelBinder para un tipo en concreto
    ///                         config.BindParameter(typeof(TestModelBinder.GeoPoint), new TestModelBinder());
    ///                     En el Controller ->
    ///                         [ModelBinder] TestModelBinder.GeoPoint location                    
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
    /// </summary>
    public class TestModelBinder : IModelBinder
    {
        // Metodo que sera llamado por Web Api para llenar un parametro del Action con una custom logica
        // En este caso se tiene un parametro con dos valores en el Query String separado por , que seran usado para llenar dos propiedades de un Complext Type
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            // Si el tipo requerido no es el soportado por este Model Binder, retornas false
            if (bindingContext.ModelType != typeof(GeoPoint))
            {
                return false;
            }

            // Invocas a los Value Providers para que hallen un valor para el nombre del parametro que Web Api esta requiriendo para llenar
            // Si no puedes obtener el valor retornas false
            ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (val == null)
            {
                return false;
            }

            string value = val.RawValue as string;
            if (value == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
                return false;
            }

            // Creas tu objeto segun el valor recuperado por los Value Providers y lo asignas al bindingContext.Model y retornando true
            GeoPoint result;
            if (GeoPoint.TryParse(value, out result))
            {
                bindingContext.Model = result;
                return true;
            }

            // Si no puedes convertirlo agregas un error al Model State y devuelves false
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value to GeoPoint");
            return false;

        }

        [ModelBinder(typeof(TestModelBinder))]
        public class GeoPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public static bool TryParse(string value, out GeoPoint result)
            {
                result = null;

                var parts = value.Split(',');
                if (parts.Length != 2)
                {
                    return false;
                }

                double latitude, longitude;
                if (double.TryParse(parts[0], out latitude) &&
                    double.TryParse(parts[1], out longitude))
                {
                    result = new GeoPoint() { Longitude = longitude, Latitude = latitude };
                    return true;
                }
                return false;
            }
        }
    }
}