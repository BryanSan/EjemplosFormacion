using System;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface IRedisCacheService : ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey) where T : class;

        Task<T> GetOrSetAsync<T>(string cacheKey, Func<T> getItemCallback) where T : class;

        Task SetAsync<T>(string cacheKey, T value) where T : class;

        Task RemoveAsync(string cacheKey);

        ICacheServiceTransaction CreateTransaction();
    }
}