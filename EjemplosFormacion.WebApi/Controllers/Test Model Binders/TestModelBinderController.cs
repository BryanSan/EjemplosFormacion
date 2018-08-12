using EjemplosFormacion.WebApi.ModelBinder;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace EjemplosFormacion.WebApi.Controllers.TestModelBinders
{
    public class TestModelBinderController : ApiController
    {
        // Metodo que usara el Model Binder especificado por el Attribute que decora la clase GeoPoint [ModelBinder(typeof(TestModelBinder))]
        public IHttpActionResult TestModelBinder(TestModelBinder.GeoPoint location)
        {
            return Ok();
        }

        // Metodo que usara el Model Binder especificado por el Attribute que acompaña al parametro del Action
        public IHttpActionResult TestModelBinderWithAttribute([ModelBinder(typeof(TestModelBinder))] TestModelBinder.GeoPoint location)
        {
            return Ok();
        }

        // Metodo que usara el Model Binedr especificado en el WebApiConfig como servicio
        // El parametro esta marcado como [ModelBinder] para que Web Api sera que este parametro sera llenado por un Model Binder o un Value Provider
        public IHttpActionResult TestModelBinderWithRegisterService([ModelBinder] TestModelBinder.GeoPoint location)
        { 
            return Ok();
        }

    }
}
