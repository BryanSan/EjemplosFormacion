using EjemplosFormacion.WebApi.DependencyResolver;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EjemplosFormacion.WebApi.UnityWebApiActivator), nameof(EjemplosFormacion.WebApi.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(EjemplosFormacion.WebApi.UnityWebApiActivator), nameof(EjemplosFormacion.WebApi.UnityWebApiActivator.Shutdown))]

namespace EjemplosFormacion.WebApi
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET.
    /// </summary>
    public static class UnityWebApiActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            // Use UnityHierarchicalDependencyResolver if you want to use
            // a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.Container);
            var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}