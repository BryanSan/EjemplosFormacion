using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;

namespace EjemplosFormacion.AzureFunction
{
    public static class TimerFunction
    {
        [FunctionName("TimerFunction")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient queueClient = cloudStorageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("myqueue-items");
            bool created = queue.CreateIfNotExistsAsync().Result;

            var message = new { Message = "Queue Message from Azure Function Timer Trigger!!!!!!" };
            string serializedMessage = JsonConvert.SerializeObject(message);

            queue.AddMessageAsync(new CloudQueueMessage(serializedMessage));
        }
    }
}