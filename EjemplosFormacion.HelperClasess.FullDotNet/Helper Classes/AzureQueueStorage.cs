using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// https://docs.microsoft.com/es-es/azure/storage/queues/storage-dotnet-how-to-use-queues
    /// </summary>
    public class AzureQueueStorage : AzureStorageBase
    {
        private readonly CloudQueueClient _cloudQueueClient;
        private readonly QueueRequestOptions _queueRequestOptions;

        public AzureQueueStorage()
        {
            _cloudQueueClient = _storageAccount.CreateCloudQueueClient();
            _cloudQueueClient.DefaultRequestOptions = new QueueRequestOptions
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 5), // Cada 2 seg pruebo
                //RetryPolicy = new NoRetry(), // No policy
                //RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 5), // Cada 2, 4, 6, 8, 10 seg pruebo (Exponencial)
                MaximumExecutionTime = TimeSpan.FromSeconds(10)
            };

            _queueRequestOptions = _cloudQueueClient.DefaultRequestOptions;
        }

        #region Queue Specific Methods
        public async Task<int?> GetQueueLenghtAsync(string queueName)
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            await queue.FetchAttributesAsync();

            int? cachedMessageCount = queue.ApproximateMessageCount;

            return cachedMessageCount;
        }

        public async Task<bool> QueueExitsAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            bool exists = await queue.ExistsAsync(_queueRequestOptions, _operationContext);

            return exists;
        }

        public async Task CreateQueueAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            await queue.CreateAsync(_queueRequestOptions, _operationContext);
        }

        public async Task<bool> CreateQueueIfNotExistsAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            bool created = await queue.CreateIfNotExistsAsync(_queueRequestOptions, _operationContext);

            return created;
        }

        public async Task DeleteQueueAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            await queue.DeleteAsync(_queueRequestOptions, _operationContext);
        }

        public async Task<bool> DeleteQueueIfExistsAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            bool deleted = await queue.DeleteIfExistsAsync(_queueRequestOptions, _operationContext);

            return deleted;
        }

        public async Task SetQueueMetadataAsync(string queueName, IDictionary<string, string> metadataProperties)
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            await queue.FetchAttributesAsync();

            foreach (KeyValuePair<string, string> metadataProperty in metadataProperties)
            {
                queue.Metadata[metadataProperty.Key] = metadataProperty.Value;
            }

            await queue.SetMetadataAsync(_queueRequestOptions, _operationContext);
        }

        public async Task<IDictionary<string, string>> GetContainerMetadata(string queueName)
        {
            CloudQueue queue = await GetQueueAsync(queueName);

            await queue.FetchAttributesAsync(_queueRequestOptions, _operationContext);

            return queue.Metadata;
        }
        #endregion

        #region Queue Message Specific Methods
        public async Task CreateQueueMessageAsync<T>(string queueName, T entity) where T : class
        {
            string serializedEntity = JsonConvert.SerializeObject(entity);
            CloudQueueMessage message = new CloudQueueMessage(serializedEntity);
            
            CloudQueue queue = await GetQueueAsync(queueName);
            await queue.AddMessageAsync(message, null, null, _queueRequestOptions, _operationContext);
        }

        public async Task<T> GetMessageAsync<T>(string queueName) where T : class
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            CloudQueueMessage message = await queue.GetMessageAsync(null, _queueRequestOptions, _operationContext);

            if (message != null)
            {
                string serializedEntity = message.AsString;
                T entity = JsonConvert.DeserializeObject<T>(serializedEntity);

                await queue.DeleteMessageAsync(message, _queueRequestOptions, _operationContext);

                return entity;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<T>> GetMessagesAsync<T>(string queueName, int numberMessages) where T : class
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            IEnumerable<CloudQueueMessage> messages = await queue.GetMessagesAsync(numberMessages, null, _queueRequestOptions, _operationContext); ;
            
            if (messages != null && messages.Count() > 0)
            {
                List<T> serializedEntities = messages.Select(x => JsonConvert.DeserializeObject<T>(x.AsString)).ToList();

                List<Task> taskList = new List<Task>();
                foreach (CloudQueueMessage message in messages)
                {
                    Task task = queue.DeleteMessageAsync(message, _queueRequestOptions, _operationContext);
                    taskList.Add(task);
                }

                await Task.WhenAll(taskList);

                return serializedEntities;
            }
            else
            {
                return null;
            }
        }

        public async Task<T> PeekMessageAsync<T>(string queueName) where T : class
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            CloudQueueMessage message = await queue.PeekMessageAsync(_queueRequestOptions, _operationContext);

            if (message != null)
            {
                string serializedEntity = message.AsString;
                T entity = JsonConvert.DeserializeObject<T>(serializedEntity);

                return entity;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<T>> PeektMessagesAsync<T>(string queueName, int numberMessages) where T : class
        {
            CloudQueue queue = await GetQueueAsync(queueName);
            IEnumerable<CloudQueueMessage> messages = await queue.GetMessagesAsync(numberMessages, null, _queueRequestOptions, _operationContext); ;

            if (messages != null && messages.Count() > 0)
            {
                List<T> serializedEntities = messages.Select(x => JsonConvert.DeserializeObject<T>(x.AsString)).ToList();

                return serializedEntities;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private Methods
        private async Task<CloudQueue> GetQueueAsync(string queueName)
        {
            CloudQueue queue = _cloudQueueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            return queue;
        }
        #endregion

    }
}