using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using System;
using System.Runtime.Caching;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class InMemoryCache : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddHours(1));
            }
            return item;
        }

        public T Get<T>(string cacheKey) where T : class
        {
            return MemoryCache.Default.Get(cacheKey) as T; 
        }

        public void Set<T>(string cacheKey, T value) where T : class
        {
            MemoryCache.Default.Set(cacheKey, value, DateTime.Now.AddHours(1));
        }
    }
}
