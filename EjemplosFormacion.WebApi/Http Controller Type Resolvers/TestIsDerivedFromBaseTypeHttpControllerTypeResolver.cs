using System;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace EjemplosFormacion.WebApi.HttpControllerTypeResolvers
{
    /// <summary>
    /// Custom Http Controller Type Resolver que se encargara de evaluar y seleccionar que clase seran las elegidas para servir de Controller
    /// Puedes customizar tu criterio para elegir las clases que quieres que sirvan de Controller
    /// En este caso estamos adicionando a los criterios ya establecidos que debe heredar de una clase base en concreto
    /// https://www.strathweb.com/2013/08/customizing-controller-discovery-in-asp-net-web-api/
    /// </summary>
    class TestIsDerivedFromBaseTypeHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        // Pasa al constructor base un metodo Delegate para que la clase base lo invoque y decida si una clase es un Controller o no
        public TestIsDerivedFromBaseTypeHttpControllerTypeResolver() : base(IsDerivedFromBaseType)
        {

        }

        // Metodo que sera invocado por la clase base para decidir si una clase es un Controller o no
        // Aqui podras customizar los criterios aplicados a las clases para que sean un Controller
        // En este caso agregamos que la clase tenga que heredar de una clase base obligatoriamente o no sera tomada en cuenta
        // Buena idea para forzar que todos los Controller hereden de una clase base
        internal static bool IsDerivedFromBaseType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return
             type != null &&
             type.IsClass &&
             type.IsVisible &&
             typeof(IHttpController).IsAssignableFrom(type) &&
             !type.IsAbstract;

            //return
            // type != null &&
            // type.IsClass &&
            // type.IsVisible &&
            // !type.IsAbstract &&
            // typeof(IHttpController).IsAssignableFrom(type) &&
            // typeof(MyBaseApiController).IsAssignableFrom(type);
        }

        /// <summary>
        /// We match if type name ends with "Controller" and that is not the only part of the 
        /// name (i.e it can't be just "Controller"). The reason is that the route name has to 
        /// be a non-empty prefix of the controller type name.
        /// </summary>
        internal static bool HasValidControllerName(Type controllerType)
        {
            if (controllerType == null) throw new ArgumentNullException("controllerType");

            string controllerSuffix = DefaultHttpControllerSelector.ControllerSuffix;

            return controllerType.Name.Length > controllerSuffix.Length && controllerType.Name.EndsWith(controllerSuffix, StringComparison.OrdinalIgnoreCase);
        }
    }
}