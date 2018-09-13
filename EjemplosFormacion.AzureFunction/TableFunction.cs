using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EjemplosFormacion.AzureFunction
{
    public static class TableFunction
    {
        [FunctionName("TableFunction")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}