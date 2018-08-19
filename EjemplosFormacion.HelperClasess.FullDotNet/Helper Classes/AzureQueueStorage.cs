using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using Microsoft.WindowsAzure.Storage.Queue;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class AzureQueueStorage : AzureStorageBase
    { 
        private readonly CloudQueueClient _cloudQueueClient;

        public AzureQueueStorage()
        {
            _cloudQueueClient = _storageAccount.CreateCloudQueueClient();
        }
    }
}