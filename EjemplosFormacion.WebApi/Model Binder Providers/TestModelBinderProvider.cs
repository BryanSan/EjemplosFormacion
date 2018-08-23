using EjemplosFormacion.WebApi.ModelBinders;
using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace EjemplosFormacion.WebApi.ModelBinderProviders
{
    /// <summary>
    /// Custom Model Binder Provider para devolver un binder segun el Type que Web Api nos pase
    /// En este caso revisamos si Web Api nos ha pasado un tipo de los soportados por el TestModelBinder
    /// Si lo es, devolvemos una instancia de el, si no devolvemos null
    /// </summary>
    public class TestModelBinderProvider : ModelBinderProvider
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            // Si es un tipo de los soportados por el Model Binder devolvemos una instancia de el
            if (TestModelBinder.CanBindType(modelType))
            {
                return new TestModelBinder();
            }

            return null;
        }
    }
}