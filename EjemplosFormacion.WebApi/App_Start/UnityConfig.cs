using EjemplosFormacion.HelperClasess.CriptographyHelpers;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Factories;
using EjemplosFormacion.HelperClasess.Json.Net.Abstract;
using EjemplosFormacion.HelperClasess.Json.Net.Factories;
using EjemplosFormacion.HelperClasess.Wrappers;
using EjemplosFormacion.HelperClasess.Wrappers.Abstract;
using EjemplosFormacion.WebApi.DirectRouteProviders;
using EjemplosFormacion.WebApi.Stubs.Abstract;
using EjemplosFormacion.WebApi.Stubs.Implementation;
using System;
using System.Security.Cryptography;
using System.Web.Http.Routing;
using Unity;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Lifetime;

namespace EjemplosFormacion.WebApi
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {

        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff660867(v%3dpandp.20)
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();

            container.RegisterType<ITestDependency, TestDependency>(new HierarchicalLifetimeManager());
            container.RegisterType<IWrapperNLog, WrapperNLogger>(new HierarchicalLifetimeManager());
            container.RegisterType<ITestDependency, TestDependency>(new HierarchicalLifetimeManager());
            container.RegisterType<IJsonSerializerSettingsFactory, JsonSerializerSettingsFactory>(new HierarchicalLifetimeManager());

            // Cryptography
            container.RegisterType<ISymmetricService<AesManaged>, SymmetricService<AesManaged>>(new HierarchicalLifetimeManager());
            container.RegisterType<ISymmetricAlgorithmFactory<AesManaged>, SymmetricAlgorithmFactory<AesManaged, SHA256Managed>>(new HierarchicalLifetimeManager(), 
                new InjectionConstructor(
                   "soy la key",
                   "soy el IVKey",
                   new ResolvedParameter<IHasher<SHA256Managed>>()
               ));            
            container.RegisterType<IHasher<SHA256Managed>, Hasher<SHA256Managed>>(new HierarchicalLifetimeManager());
            container.RegisterType<IHashAlgorithmFactory<SHA256Managed>, HashAlgorithmFactory<SHA256Managed>>(new HierarchicalLifetimeManager());

            container.RegisterType<IDirectRouteProvider, TestGlobalPrefixDirectRouteProvider>(new TransientLifetimeManager(), new InjectionConstructor("api"));
        }
    }
}