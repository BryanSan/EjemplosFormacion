using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using System;
using System.Web.Caching;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// Custom Class que usa el Built-In MemoryCache de .Net para las aplicaciones Web
    /// Tiene las clasicas funciones de CRUD y te permite guardar cualquier tipo de dato
    /// Adicionalmente te permite usar la clase CacheDependency para crear una entrada de Cache que su expiracion depende con que una File o Directory cambie en el servidor
    /// Digamos que sirve para cachear la informacion de un archivo y le pones un CacheDependency para que el cache solo se Expire cuando este archivo cambie
    /// RECORDAR QUE ESTO ES PARA APLICACIONES DE ASP.NET O TE PUEDE DAR ERROR SI LO USAS EN OTRO TIPO DE APLICACION
    /// </summary>
    public class InMemoryCacheWeb : IFileDependencyCacheService
    {

        private readonly Cache _cache;

        public InMemoryCacheWeb()
        {
            _cache = new Cache();
        }

        /// <summary>
        /// Metodo solo para obtener el cache
        /// </summary>
        public T Get<T>(string cacheKey) where T : class
        {
            T cachedObject = _cache.Get(cacheKey) as T;
            return cachedObject;
        }

        /// <summary>
        /// Metodo para obtener o simplemente si existe en el cache
        /// Si no existe en el cache usara el Func<T> para obtener el valor, luego guardarlo en el cache y lo devuelve
        /// </summary>
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T cacheObject = Get<T>(cacheKey);
            if (cacheObject == null)
            {
                cacheObject = getItemCallback();
                Set(cacheKey, cacheObject);
            }

            return cacheObject;
        }

        /// <summary>
        /// Metodo para obtener o simplemente si existe en el cache
        /// Si no existe en el cache usara el Func<T> para obtener el valor, luego guardarlo en el cache y lo devuelve
        /// Agregandole una dependencia al cache a un File o Directory especificado en el filePathDependency parameter
        /// Cuando este cambie, el cache es invalidado y volvera a recargarse
        /// </summary>
        public T GetOrSet<T>(string cacheKey, string filePathDependency, Func<T> getItemCallback) where T : class
        {
            T cacheObject = Get<T>(cacheKey);
            if (cacheObject == null)
            {
                cacheObject = getItemCallback();
                Set(cacheKey, cacheObject, filePathDependency);
            }

            return cacheObject;
        }

        /// <summary>
        /// Metodo solo para setear el cache
        /// </summary>
        public void Set<T>(string cacheKey, T value) where T : class
        {
            _cache.Insert(cacheKey, value, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Metodo solo para setear el cache 
        /// Agregandole una dependencia al cache a un File o Directory especificado en el filePathDependency parameter
        /// Cuando este cambie, el cache es invalidado y volvera a recargarse
        /// </summary>
        public void Set<T>(string cacheKey, T value, string filePathDependency) where T : class
        {
            var cacheDependency = new CacheDependency(filePathDependency);
            _cache.Insert(cacheKey, value, cacheDependency);
        }

        /// <summary>
        /// Metodo solo para remover un object segun su key
        /// </summary>
        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }
    }
}
