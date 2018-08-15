using System;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface ICacheService : IDisposable
    {
        T Get<T>(string cacheKey) where T : class;

        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;

        void Set<T>(string cacheKey, T value) where T : class;

        void Remove(string cacheKey);
    }
}
