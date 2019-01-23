using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using Microsoft.WindowsAzure.Storage;
using StackExchange.Redis;
using System.Configuration;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// Clase usada para hacer una abstraction al Type Batch devuelta por Redis
    /// De manera que no expongamos los types de la libreria si no los nuestros propios
    /// </summary>
    public class RedisBatch : ICacheServiceBatch
    {

        private readonly IBatch _batchRedis;

        // Recibimos el tipo Batch de Redis y lo guardamos para su posterior uso
        public RedisBatch(IBatch batchRedis)
        {
            _batchRedis = batchRedis;
        }

        // Commiteamos en el Tipo Batch de Redis
        public void Execute()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            _batchRedis.Execute();
        }
    }
}
