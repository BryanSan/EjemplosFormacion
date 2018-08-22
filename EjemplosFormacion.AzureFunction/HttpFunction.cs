using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;

namespace EjemplosFormacion.AzureFunction
{
    /// <summary>
    /// Azure Function que seran ejecutadas mediante peticion Http, puedes configurarla y hacerle Fine-Tune en el constructor del HttpTriggerAttribute y sus propiedades
    /// https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook
    /// </summary>
    public static class HttpFunction
    {
        // Nombre unico que denota como se llama la Function y a donde apuntara el HttpRequest
        [FunctionName(nameof(HttpFunctionWithNoBinding))] 
        // Recordad que cada Trigger funciona con varios tipos de datos no estas atado al HttpRequest, puedes inspeccionar la documentacion y ver que demas tipos soporta
        public static IActionResult HttpFunctionWithNoBinding([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function {nameof(HttpFunctionWithNoBinding)} processed a request.");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient queueClient = cloudStorageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("queueTest");
            bool created = queue.CreateIfNotExistsAsync().Result;

            var message = new { Message = "Queue Message from Azure Function Timer Trigger!!!!!!" };
            string serializedMessage = JsonConvert.SerializeObject(message);

            queue.AddMessageAsync(new CloudQueueMessage(serializedMessage));
            return new OkResult();
        }

        // Nombre unico que denota como se llama la Function y a donde apuntara el HttpRequest
        [FunctionName(nameof(HttpFunctionWithReturnValueBinding))]
        // Especifica un Output Binding a una Queue de Storage denotada con el nombre queueTest y que este en el Storage especificado en el ConnectionString 
        // En Castellano, lo que este metodo devuelva ira a parar a una Queue de Storage con el nombre queueTest (estamos insertando un mensaje en la cola)
        // Estas definiendo el Output Binding con un Attribute en este caso
        [return: Queue("queueTest", Connection = "AzureWebJobsStorage")]
        // Recordad que cada Trigger funciona con varios tipos de datos no estas atado al HttpRequest, puedes inspeccionar la documentacion y ver que demas tipos soporta
        public static Person HttpFunctionWithReturnValueBinding([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function {nameof(HttpFunctionWithReturnValueBinding)} processed a request.");

            return new Person()
            {
                PartitionKey = "Orders",
                RowKey = Guid.NewGuid().ToString(),
                Name = "Name",
                MobileNumber = "MobileNumber"
            };
        }

        // Nombre unico que denota como se llama la Function y a donde apuntara el HttpRequest
        [FunctionName(nameof(HttpFunctionWithOutputBinding))]
        // Especifica un Output Binding a una Queue de Storage denotada con el nombre queueTest y que este en el Storage especificado en el ConnectionString 
        // En Castellano, lo que este metodo devuelva ira a parar a una Queue de Storage con el nombre queueTest (estamos insertando un mensaje en la cola)
        // Estas definiendo el Output Binding con un Attribute en este caso especificandolo como de tipo out
        // Debes asignar esta variable antes de salir del metodo
        // Recordad que cada Trigger funciona con varios tipos de datos no estas atado al HttpRequest, puedes inspeccionar la documentacion y ver que demas tipos soporta
        public static void HttpFunctionWithOutputBinding([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
                                                         [Queue("queueTest", Connection = "AzureWebJobsStorage")] out Person myQueueItemCopy,
                                                         ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function {nameof(HttpFunctionWithOutputBinding)} processed a request.");

            myQueueItemCopy = new Person()
            {
                PartitionKey = "Orders",
                RowKey = Guid.NewGuid().ToString(),
                Name = "Name",
                MobileNumber = "MobileNumber"
            };
        }
    }
}