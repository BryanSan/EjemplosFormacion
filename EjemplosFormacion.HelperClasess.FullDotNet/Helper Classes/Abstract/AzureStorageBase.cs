using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract
{
    public abstract class AzureStorageBase
    {
        protected readonly CloudStorageAccount _storageAccount;
        protected readonly OperationContext _operationContext;
        

        // <appSettings>
        //      <add key = "StorageConnection" value="DefaultEndpointsProtocol=https;AccountName=account-name;AccountKey=account-key" />
        // </appSettings>
        public AzureStorageBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            _operationContext = new OperationContext();
        }
    }
}
