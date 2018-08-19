using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Linq;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// https://docs.microsoft.com/es-es/azure/cosmos-db/table-storage-how-to-use-dotnet?toc=%2Fes-es%2Fazure%2Fstorage%2Ftables%2FTOC.json&bc=%2Fes-es%2Fazure%2Fbread%2Ftoc.json
    /// </summary>
    public class AzureTableStorage
    {

        private readonly CloudTableClient _cloudTableClient;

        // <appSettings>
        //      <add key = "StorageConnection" value="DefaultEndpointsProtocol=https;AccountName=account-name;AccountKey=account-key" />
        // </appSettings>
        public AzureTableStorage()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _cloudTableClient = storageAccount.CreateCloudTableClient();
        }

        #region Table Specific Methods
        public async Task CreateTableAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);

            await table.CreateAsync();
        }

        public async Task<bool> CreateTableIfNotExistAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool created = await table.CreateIfNotExistsAsync();

            return created;
        }

        public async Task<bool> TableExistAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool exists = await table.ExistsAsync();

            return exists;
        }

        public async Task DeleteAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);

            await table.DeleteAsync();
        }

        public async Task<bool> DeleteIfExistsAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            bool deleted = await table.DeleteIfExistsAsync();

            return deleted;
        }
        #endregion

        #region Entity Specific Methods
        public async Task InsertAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation insertOperation = TableOperation.Insert(entity);

            TableResult result = await table.ExecuteAsync(insertOperation);
        }

        public async Task InsertOrReplaceAsync<T>(string tableName, string partitionKey, string rowKey, T entity) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            await table.ExecuteAsync(insertOrReplaceOperation);
        }

        public async Task InsertBatchAsync<T>(string tableName, List<T> entities) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            var batchOperation = new TableBatchOperation();
            foreach (T entity in entities)
            {
                batchOperation.Insert(entity);
            }

            IList<TableResult> results = await table.ExecuteBatchAsync(batchOperation);
        }

        public async Task<List<T>> FindByAsync<T>(string tableName) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), null);

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
                TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

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

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, null);

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
                TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

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

            TableResult result = await table.ExecuteAsync(retrieveOperation);

            return result.Result as T;
        }

        public async Task DeleteByAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(retrieveOperation);

            T retrievedEntity = result.Result as T;

            if (retrievedEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(retrievedEntity);

                await table.ExecuteAsync(deleteOperation);
            }
        }

        public async Task DeleteByAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new()
        {
            CloudTable table = await GetTableAsync(tableName);

            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableQuerySegment<T> result = await table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());

            List<T> retrievedEntities = result.Results;

            if (retrievedEntities != null && retrievedEntities.Count > 0)
            {
                var batchOperation = new TableBatchOperation();

                foreach (T retrievedEntity in retrievedEntities)
                {
                    batchOperation.Delete(retrievedEntity);
                }

                IList<TableResult> results = await table.ExecuteBatchAsync(batchOperation);
            }
        }
        #endregion

        #region Private Methods
        private async Task<CloudTable> GetTableAsync(string tableName)
        {
            CloudTable table = _cloudTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
        #endregion

    }
}