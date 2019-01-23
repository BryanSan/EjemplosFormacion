using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using System;
using System.Runtime.Caching;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// Custom Class que usa el Built-In MemoryCache de .Net
    /// Tiene las clasicas funciones de CRUD y te permite guardar cualquier tipo de dato
    /// https://exceptionnotfound.net/a-simple-caching-scheme-for-web-api-using-dependency-injection/
    /// </summary>
    public class InMemoryCache : ICacheService
    {
        /// <summary>
        /// Metodo solo para obtener el cache
        /// </summary>
        public T Get<T>(string cacheKey) where T : class
        {
            T cachedObject = MemoryCache.Default.Get(cacheKey) as T;
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
        /// Metodo solo para setear el cache
        /// </summary>
        public void Set<T>(string cacheKey, T value) where T : class
        {
            MemoryCache.Default.Set(cacheKey, value, DateTime.Now.AddHours(1));
        }

        /// <summary>
        /// Metodo solo para remover un object segun su key
        /// </summary>
        public void Remove(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
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
                MemoryCache.Default.Dispose();
            }
        }
    }
}
