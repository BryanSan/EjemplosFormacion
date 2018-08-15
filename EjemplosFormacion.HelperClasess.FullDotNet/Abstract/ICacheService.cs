using System;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;

        T Get<T>(string cacheKey) where T : class;

        void Set<T>(string cacheKey, T value) where T : class;
    }
}
