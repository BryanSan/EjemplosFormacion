using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace EjemplosFormacion.WebApi.AssembliesResolver
{
    /// <summary>
    /// Custom Implementacion del servicio IAssembliesResolver en el cual Web Api se apoya para resolver cuales Assemblies son parte de su servicio
    /// Por ejemplo a la hora de buscar los Controllers disponibles, Web Api se apoya en este servicio para obtener los Assembies en el cual buscar estos Controllers
    /// </summary>
    class TestInternalAndExternalAssembliesResolver : IAssembliesResolver
    {
        // Metodo que sera llamado por Web Api para obtener todos los Assemblies que participan en el servicio
        public virtual ICollection<Assembly> GetAssemblies()
        {
            // Carga todos los Assemblies del current App Domain
            // En este caso todo lo que se copie en la carpeta bin estara aca
            // Como el proyecto actual y sus referencias
            ICollection<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            
            // Aqui buscamos y cargamos todos los demas assemblies que querramos agregar a los defaults
            //var controllersAssembly = Assembly.LoadFrom("c:/myAssymbly.dll");
            //baseAssemblies.Add(controllersAssembly);

            return baseAssemblies;
        }
    }
}