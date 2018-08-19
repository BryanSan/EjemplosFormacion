using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// Clase usada para hacer una abstraction al Type Transaccion devuelto por Redis
    /// De manera que no expongamos los types de la libreria si no los nuestros propios
    /// </summary>
    public class RedisTransaction : ICacheServiceTransaction
    {
        
        private readonly ITransaction _redisTransaction;

        // Recibimos el tipo Transaction de Redis y lo guardamos para su posterior uso
        public RedisTransaction(ITransaction redisTransaction)
        {
            _redisTransaction = redisTransaction;
        }

        // Commiteamos en el Tipo Transaction de Redis de manera sync
        public bool Commit()
        {
            bool commited =  _redisTransaction.Execute();
            return commited;
        }

        // Commiteamos en el Tipo Transaction de Redis de manera async
        public async Task<bool> CommitAsync()
        {
            bool commited = await _redisTransaction.ExecuteAsync();
            return commited;
        }
    }
}
