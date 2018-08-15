using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// Custom Class que usa el Nuget para trabajar con base de datos InMemory de Redis
    /// Tiene las clasicas funciones de CRUD y te permite guardar solo strings
    /// Por lo tanto si quieres guardar algun ComplexObject una solucion seria serializarlo a Json y guardarlo como string
    /// Al mismo tiempo da soporte para Transacciones y operaciones Batch
    /// https://stackexchange.github.io/StackExchange.Redis/
    /// </summary>
    public class InMemoryCacheRedis : IRedisCacheService
    {

        private readonly IDatabase _redisDataBase;
        private readonly ConnectionMultiplexer _redisConnection;

        public InMemoryCacheRedis()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;

            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
            _redisDataBase = _redisConnection.GetDatabase();
        }

        /// <summary>
        /// Metodo solo para obtener el cache sync
        /// </summary>
        public T Get<T>(string cacheKey) where T : class
        {
            string serializedEntity = _redisDataBase.StringGet(cacheKey);
            T entity = JsonConvert.DeserializeObject<T>(serializedEntity);

            return entity;
        }

        /// <summary>
        /// Metodo solo para obtener el cache async
        /// </summary>
        public async Task<T> GetAsync<T>(string cacheKey) where T : class
        {
            string serializedEntity = await _redisDataBase.StringGetAsync(cacheKey);
            T entity = JsonConvert.DeserializeObject<T>(serializedEntity);

            return entity;
        }

        /// <summary>
        /// Metodo para obtener o simplemente si existe en el cache sync
        /// Si no existe en el cache usara el Func<T> para obtener el valor, luego guardarlo en el cache y lo devuelve
        /// </summary>
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T cachedObject = Get<T>(cacheKey);
            if (cachedObject == null)
            {
                cachedObject = getItemCallback();
                Set(cacheKey, cachedObject);
            }

            return cachedObject;
        }

        /// <summary>
        /// Metodo para obtener o simplemente si existe en el cache async
        /// Si no existe en el cache usara el Func<T> para obtener el valor, luego guardarlo en el cache y lo devuelve
        /// </summary>
        public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T cachedObject = await GetAsync<T>(cacheKey);
            if (cachedObject == null)
            {
                cachedObject = getItemCallback();
                await SetAsync(cacheKey, cachedObject);
            }

            return cachedObject;
        }

        /// <summary>
        /// Metodo solo para setear el cache async
        /// </summary>
        public async Task SetAsync<T>(string cacheKey, T value) where T : class
        {
            string serializedEntity = JsonConvert.SerializeObject(value);

            await _redisDataBase.StringSetAsync(cacheKey, serializedEntity);
        }

        /// <summary>
        /// Metodo solo para setear el cache async
        /// </summary>
        public void Set<T>(string cacheKey, T value) where T : class
        {
            string serializedEntity = JsonConvert.SerializeObject(value);

            _redisDataBase.StringSet(cacheKey, serializedEntity);
        }

        /// <summary>
        /// Metodo solo para remover un object segun su key sync
        /// </summary>
        public void Remove(string cacheKey)
        {
            _redisDataBase.KeyDelete(cacheKey);
        }

        /// <summary>
        /// Metodo solo para remover un object segun su key async
        /// </summary>
        public async Task RemoveAsync(string cacheKey)
        {
            await _redisDataBase.KeyDeleteAsync(cacheKey);
        }

        /// <summary>
        /// Metodo para crear una transaccion
        /// Has las operaciones y luego llama al metodo Commit del objeto transaccion para commitear las operaciones
        /// </summary>
        public ICacheServiceTransaction CreateTransaction()
        {
            ITransaction transaction = _redisDataBase.CreateTransaction();
            var redisTransaction = new RedisTransaction(transaction);

            return redisTransaction;
        }

        /// <summary>
        /// Metodo para crear un batch
        /// Has las operaciones y luego llama al metodo Execute del objeto batch para executas las operaciones en batch
        /// </summary>
        public ICacheServiceBatch CreateBatch()
        {
            IBatch batch = _redisDataBase.CreateBatch();
            var redisBatch = new RedisBatch(batch);

            return redisBatch;
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
                if (_redisConnection != null)
                {
                    _redisConnection.Dispose();
                }
            }
        }
    }
}
