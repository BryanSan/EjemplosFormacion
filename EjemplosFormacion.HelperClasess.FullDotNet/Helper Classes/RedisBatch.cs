using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using StackExchange.Redis;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class RedisBatch : ICacheServiceBatch
    {

        private readonly IBatch _batchRedis;

        public RedisBatch(IBatch batchRedis)
        {
            _batchRedis = batchRedis;
        }

        public void Execute()
        {
            _batchRedis.Execute();
        }
    }
}
