using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace EjemplosFormacion.AzureFunction
{
    public static class HttpFunction
    {
        [FunctionName("HttpFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient queueClient = cloudStorageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("myqueue-items");
            bool created = queue.CreateIfNotExistsAsync().Result;

            var message = new { Message = "Queue Message from Azure Function Timer Trigger!!!!!!" };
            string serializedMessage = JsonConvert.SerializeObject(message);

            queue.AddMessageAsync(new CloudQueueMessage(serializedMessage));
            return new OkResult();
        }
    }
}