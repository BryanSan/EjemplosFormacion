using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;

namespace EjemplosFormacion.AzureFunction
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob
    /// </summary>
    public static class BlobFunction
    {
        [FunctionName("BlobFunction")]
        public static void Run([BlobTrigger("samples-workitems/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}