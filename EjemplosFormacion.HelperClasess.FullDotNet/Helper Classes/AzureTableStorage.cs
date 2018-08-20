using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// https://docs.microsoft.com/es-es/azure/cosmos-db/table-storage-how-to-use-dotnet?toc=%2Fes-es%2Fazure%2Fstorage%2Ftables%2FTOC.json&bc=%2Fes-es%2Fazure%2Fbread%2Ftoc.json
    /// </summary>
    public class AzureTableStorage : AzureStorageBase
    {

        private readonly CloudTableClient _cloudTableClient;
        private readonly TableRequestOptions _tableRequestOptions;
        private readonly AccessCondition _accesCondition;

        public AzureTableStorage() : base()
        {
            _cloudTableClient = _storageAccount.CreateCloudTableClient();
            _tableRequestOptions = new TableRequestOptions
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 5),
                MaximumExecutionTime = TimeSpan.FromSeconds(10)
            };
            _accesCondition = AccessCondition.GenerateEmptyCondition();
        }

        #region Table Specific Methods
        public async Task<bool> TableExistAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool exists = await table.ExistsAsync(_tableRequestOptions, _operationContext);

            return exists;
        }

        public async Task CreateTableAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);

            await table.CreateAsync(_tableRequestOptions, _operationContext);
        }

        public async Task<bool> CreateTableIfNotExistAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool created = await table.CreateIfNotExistsAsync(_tableRequestOptions, _operationContext);

            return created;
        }

        public async Task DeleteAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);

            await table.DeleteAsync(_tableRequestOptions, _operationContext);
        }

        public async Task<bool> DeleteIfExistsAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool deleted = await table.DeleteIfExistsAsync(_tableRequestOptions, _operationContext);

            return deleted;
        }
        #endregion

        #region Entity Specific Methods
        public async Task InsertAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation insertOperation = TableOperation.Insert(entity);

            TableResult result = await table.ExecuteAsync(insertOperation, _tableRequestOptions, _operationContext);
        }

        public async Task InsertOrReplaceAsync<T>(string tableName, string partitionKey, string rowKey, T entity) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            await table.ExecuteAsync(insertOrReplaceOperation, _tableRequestOptions, _operationContext);
        }

        public async Task InsertBatchAsync<T>(string tableName, List<T> entities) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            var batchOperation = new TableBatchOperation();
            foreach (T entity in entities)
            {
                batchOperation.Insert(entity);
            }

            IList<TableResult> results = await table.ExecuteBatchAsync(batchOperation, _tableRequestOptions, _operationContext);
        }

        public async Task<List<T>> FindByAsync<T>(string tableName) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), null, _tableRequestOptions, _operationContext);

            return result.Results;
        }

        public async Task<List<T>> FindByPagingAsync<T>(string tableName, Action<List<T>> actionPerPage = null) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuery<T> query = new TableQuery<T>();

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            List<T> resultAllQuery = new List<T>();
            do
            {
                TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, continuationToken, _tableRequestOptions, _operationContext);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = result.ContinuationToken;

                resultAllQuery.AddRange(result.Results);

                if (actionPerPage != null)
                {
                    actionPerPage(result.Results);
                }
            } while (continuationToken != null);

            return resultAllQuery;
        }

        public async Task<List<T>> FindByAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, null, _tableRequestOptions, _operationContext);

            return result.Results;
        }

        public async Task<List<T>> FindByPagingAsync<T>(string tableName, string partitionKey, Action<List<T>> actionPerPage = null) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            List<T> resultAllQuery = new List<T>();
            do
            {
                TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, continuationToken, _tableRequestOptions, _operationContext);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = result.ContinuationToken;

                resultAllQuery.AddRange(result.Results);

                if (actionPerPage != null)
                {
                    actionPerPage(result.Results);
                }
            } while (continuationToken != null);

            return resultAllQuery;
        }

        public async Task<T> FindByAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(retrieveOperation, _tableRequestOptions, _operationContext);

            return result.Result as T;
        }

        public async Task DeleteByAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(retrieveOperation, _tableRequestOptions, _operationContext);

            T retrievedEntity = result.Result as T;

            if (retrievedEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(retrievedEntity);

                await table.ExecuteAsync(deleteOperation, _tableRequestOptions, _operationContext);
            }
        }

        public async Task DeleteByAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken(), _tableRequestOptions, _operationContext);

            List<T> retrievedEntities = result.Results;

            if (retrievedEntities != null && retrievedEntities.Count > 0)
            {
                var batchOperation = new TableBatchOperation();

                foreach (T retrievedEntity in retrievedEntities)
                {
                    batchOperation.Delete(retrievedEntity);
                }

                IList<TableResult> results = await table.ExecuteBatchAsync(batchOperation, _tableRequestOptions, _operationContext);
            }
        }
        #endregion

        #region Private Methods
        private async Task<CloudTable> GetTableAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync(_tableRequestOptions, _operationContext);
            return table;
        }
        #endregion

    }
}