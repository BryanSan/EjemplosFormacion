using System;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface IFileDependencyCacheService : ICacheService
    {
        T GetOrSet<T>(string cacheKey, string filePathDependency, Func<T> getItemCallback) where T : class;
        void Set<T>(string cacheKey, T value, string filePathDependency) where T : class;
    }
}
