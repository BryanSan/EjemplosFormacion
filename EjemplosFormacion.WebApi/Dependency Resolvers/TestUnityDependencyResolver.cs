using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace EjemplosFormacion.WebApi.DependencyResolvers
{
    /// <summary>
    /// Dependency Resolver para ayudar al Web Api a resolver las dependencias de los controladores que cree, 
    /// ya que nosotros no creamos el controlador si no es el Web Api quien lo hace, por esta razon necesita de la ayuda de un Dependency Resolver para resolver esas dependencias
    /// When Web API creates a controller instance, it first calls IDependencyResolver.GetService, passing in the controller type. 
    /// You can use this extensibility hook to create the controller, resolving any dependencies. 
    /// If GetService returns null, Web API looks for a parameterless constructor on the controller class.
    /// No retornar errores si no se puede resolver las dependencias, simplemente devolver null
    /// https://docs.microsoft.com/es-es/aspnet/web-api/overview/advanced/dependency-injection
    /// </summary>
    public class TestUnityDependencyResolver : IDependencyResolver
    {
        protected readonly IUnityContainer _container;

        public TestUnityDependencyResolver(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentException("container vacio!.");
        }

        /// <summary>
        /// Pasa el Type que quieres crear para resolverle las dependencias y devolverlo creado completo junto con sus dependencias
        /// No necesariamente debe tener dependencias para pasar por aca, todo objeto que Web Api cree pasara por aca
        /// Por lo tanto, si ves que hay Types raros que tu no has registrado, no pasa nada, devolver null y Web Api buscara un constructor vacio para crear lo que necesita
        /// </summary>
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Pasa el Type que quieres crear para resolverle las dependencias y devolverlo creado completo junto con sus dependencias
        /// No necesariamente debe tener dependencias para pasar por aca, todo objeto que Web Api cree pasara por aca
        /// Por lo tanto, si ves que hay Types raros que tu no has registrado, no pasa nada, devolver null y Web Api buscara un constructor vacio para crear lo que necesita
        /// </summary>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// El Dependency Resolver registrado en el WebApiConfig tiene Scope Global 
        /// Para el manejo de LifeTime Objects, Web Api usa el Dependency Resolver Global para crear un Child Container cuando se crean los Controller
        /// Este Child Container sera el encargado de crear el Controller y sus Dependencias
        /// Esto es para que luego que el Request termine y ya no se necesite mas el Controller, entonces se le haga un Dipose al Controller, a sus dependencias y al Container creado exclusivamente para el
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            IUnityContainer childContainer = _container.CreateChildContainer();
            return new TestUnityDependencyResolver(childContainer);
        }

        /// <summary>
        /// Dispose al Container, al hacerle Dispose al Container sus dependencias que fueron creadas por el se les hace Dispose tambien
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_container != null)
                {
                    _container.Dispose();
                }
            }
        }

    }
}