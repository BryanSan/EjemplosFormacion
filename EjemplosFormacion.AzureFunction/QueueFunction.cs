using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace EjemplosFormacion.AzureFunction
{
    /// <summary>
    /// The storage account to use is determined in the following order:
    /// The QueueTrigger attribute's Connection property.
    /// The StorageAccount attribute applied to the same parameter as the QueueTrigger attribute.
    /// The StorageAccount attribute applied to the function.
    /// The StorageAccount attribute applied to the class.
    /// The "AzureWebJobsStorage" app setting.
    /// https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue
    /// </summary>
    // Especificar que Storage Account usar para toda la clase
    [StorageAccount("AzureWebJobsStorage")] 
    public static class QueueFunction
    {
        // Especificar que Storage Account usar para toda la Azure Function
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName("QueueFunction")]
        public static void Run([StorageAccount("AzureWebJobsStorage")] // Especificar que Storage Account usar para el parametro en especifico
                               [QueueTrigger("queueTest", Connection = "AzureWebJobsStorage")] // Tambien puedes especificar el Storage Account para el paramero en especificdo en la propiedad Connection del QueueTriggerAttribute
                               string myQueueItem, 
                               ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}