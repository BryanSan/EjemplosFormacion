using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class RedisTransaction : ICacheServiceTransaction
    {

        private readonly ITransaction _redisTransaction;

        public RedisTransaction(ITransaction redisTransaction)
        {
            _redisTransaction = redisTransaction;
        }

        public bool Commit()
        {
            bool commited =  _redisTransaction.Execute();
            return commited;
        }

        public async Task<bool> CommitAsync()
        {
            bool commited = await _redisTransaction.ExecuteAsync();
            return commited;
        }
    }
}
