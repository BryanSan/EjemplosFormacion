using System;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract
{
    public interface IRedisCacheService : ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey) where T : class;

        Task<T> GetOrSetAsync<T>(string cacheKey, Func<T> getItemCallback) where T : class;

        Task SetAsync<T>(string cacheKey, T value) where T : class;

        Task RemoveAsync(string cacheKey);

        ICacheServiceTransaction CreateTransaction();

        ICacheServiceBatch CreateBatch();

        void Suscribe<T>(string key, Action<string, T> action) where T : class;

        Task SuscribeAsync<T>(string key, Action<string, T> action) where T : class;

        void Publish<T>(string key, T message) where T : class;

        Task PublishAsync<T>(string key, T message) where T : class;
    }
}