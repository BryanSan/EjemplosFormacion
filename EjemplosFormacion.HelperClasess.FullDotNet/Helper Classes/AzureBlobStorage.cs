﻿using EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    /// <summary>
    /// https://docs.microsoft.com/es-es/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=windows
    /// </summary>
    public class AzureBlobStorage : AzureStorageBase
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly BlobRequestOptions _blobRequestOptions;
        private readonly AccessCondition _accesCondition;

        public AzureBlobStorage()
        {
            _cloudBlobClient = _storageAccount.CreateCloudBlobClient();
            _cloudBlobClient.DefaultRequestOptions = new BlobRequestOptions
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 5),
                //RetryPolicy = new NoRetry(), // No policy
                //RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 5), // Cada 2, 4, 6, 8, 10 seg pruebo (Exponencial)
                MaximumExecutionTime = TimeSpan.FromSeconds(10)
            };

            _blobRequestOptions = _cloudBlobClient.DefaultRequestOptions;
            _accesCondition = AccessCondition.GenerateEmptyCondition();
        }

        #region Container Specific Methods
        public async Task<bool> ContainerExitsAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            bool exists = await container.ExistsAsync(_blobRequestOptions, _operationContext);

            return exists;
        }

        public async Task CreateContainerAsync(string containerName, BlobContainerPublicAccessType accessType)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);

            await container.CreateAsync(accessType, _blobRequestOptions, _operationContext);
        }

        public async Task<bool> CreateContainerIfNotExistAsync(string containerName, BlobContainerPublicAccessType accessType)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            bool created = await container.CreateIfNotExistsAsync(accessType, _blobRequestOptions, _operationContext);

            return created;
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            await container.DeleteAsync(_accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<bool> DeleteContainerIfExistAsync(string containerName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            bool deleted = await container.DeleteIfExistsAsync(_accesCondition, _blobRequestOptions, _operationContext);

            return deleted;
        }

        public async Task SetContainerMetadata(string containerName, IDictionary<string, string> metadataProperties)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            await container.FetchAttributesAsync();

            foreach (KeyValuePair<string, string> metadataProperty in metadataProperties)
            {
                container.Metadata[metadataProperty.Key] = metadataProperty.Value;
            }

            await container.SetMetadataAsync(_accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<IDictionary<string, string>> GetContainerMetadata(string containerName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);

            await container.FetchAttributesAsync(_accesCondition, _blobRequestOptions, _operationContext);

            return container.Metadata;
        }

        public async Task<string> GetContainerSharedAccessSignature(string containerName, string blobName, SharedAccessBlobPermissions sharedAccesType)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);

            string sas = container.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = sharedAccesType,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(5)
            });
            return sas;
        }

        public async Task<string> GetContainerharedAccessSignature(string containerName, string blobName, string policyName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);

            string sas = container.GetSharedAccessSignature(new SharedAccessBlobPolicy(), policyName);
            return sas;
        }

        public async Task SetContainerPermissionAsync(string containerName, BlobContainerPublicAccessType accessType)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);

            BlobContainerPermissions permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = accessType;

            await container.SetPermissionsAsync(permissions, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task SetContainerSharedAccessPermissionAsync(string containerName, string policyName, SharedAccessBlobPermissions sharedAccesType)
        {
            var sharedPolicy = new SharedAccessBlobPolicy
            {
                Permissions = sharedAccesType
            };

            CloudBlobContainer container = await GetContainerAsync(containerName);
            BlobContainerPermissions permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = BlobContainerPublicAccessType.Off;

            if (permissions.SharedAccessPolicies.ContainsKey(policyName))
            {
                permissions.SharedAccessPolicies.Remove(policyName);
            }
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);

            await container.SetPermissionsAsync(permissions, _accesCondition, _blobRequestOptions, _operationContext);
        }
        #endregion

        #region Blob Specific Methods
        public async Task<string> GetBlobSharedAccessSignature(string containerName, string blobName, SharedAccessBlobPermissions sharedAccesType)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            string sas = cloudBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = sharedAccesType,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(5)
            });
            return sas;
        }

        public async Task<string> GetBlobSharedAccessSignature(string containerName, string blobName, string policyName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            string sas = cloudBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy(), policyName);
            return sas;
        }

        public async Task<bool> BlobExistsAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            bool exists = await cloudBlob.ExistsAsync(_blobRequestOptions, _operationContext);

            return exists;
        }

        public async Task SetBlobMetadata(string containerName, string blobName, IDictionary<string, string> metadataProperties)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);
            await cloudBlob.FetchAttributesAsync();

            foreach (KeyValuePair<string, string> metadataProperty in metadataProperties)
            {
                cloudBlob.Metadata[metadataProperty.Key] = metadataProperty.Value;
            }

            await cloudBlob.SetMetadataAsync(_accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<IDictionary<string, string>> GetBlobMetadata(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            await cloudBlob.FetchAttributesAsync(_accesCondition, _blobRequestOptions, _operationContext);

            return cloudBlob.Metadata;
        }

        public async Task DonwloadBlobAsync(string containerName, string blobName, string destinationFilePath, FileMode fileMode)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            await cloudBlob.DownloadToFileAsync(destinationFilePath, fileMode, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task DonwloadBlobAsync(string containerName, string blobName, Stream destinationStream)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            await cloudBlob.DownloadToStreamAsync(destinationStream, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task DonwloadBlobAsync(string containerName, string blobName, byte[] target, int index = 0)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            await cloudBlob.DownloadToByteArrayAsync(target, index, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<Stream> GetBlobReadStreamAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            Stream blobStream = await cloudPageBlob.OpenReadAsync(_accesCondition, _blobRequestOptions, _operationContext);

            return blobStream;
        }

        public async Task<IAzureReadOnlyBlob> GetBlobAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);
            await cloudBlob.FetchAttributesAsync(_accesCondition, _blobRequestOptions, _operationContext);
            
            var readOnlyBlob = new AzureReadOnlyBlob(cloudBlob);

            return readOnlyBlob;
        }

        public async Task<List<Uri>> ListBlobAsync(string containerName, bool useFlatBlobListing = false, string prefix = null)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);

            var listUris = new List<Uri>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                BlobResultSegment results = await container.ListBlobsSegmentedAsync(prefix, useFlatBlobListing, BlobListingDetails.None, null, blobContinuationToken, _blobRequestOptions, _operationContext);

                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                listUris.AddRange(results.Results.Select(x => x.Uri));
            } while (blobContinuationToken != null);

            return listUris;
        }

        public async Task<List<Uri>> ListBlobAsync(string containerName, string directoryPath, bool useFlatBlobListing = false)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlobDirectory directory = container.GetDirectoryReference(directoryPath);

            List<Uri> listUris = new List<Uri>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                BlobResultSegment results = await directory.ListBlobsSegmentedAsync(useFlatBlobListing, BlobListingDetails.None, null, blobContinuationToken, _blobRequestOptions, _operationContext);

                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                listUris.AddRange(results.Results.Select(x => x.Uri));
            } while (blobContinuationToken != null);

            return listUris;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            await cloudBlob.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<bool> DeleteBlobIfExistAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob cloudBlob = container.GetBlobReference(blobName);

            bool deleted = await cloudBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, _accesCondition, _blobRequestOptions, _operationContext);

            return deleted;
        }
        #endregion

        #region Block Blob Specific Methods
        public async Task UploadBlockBlobAsync(string containerName, string blobName, string filePath)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);

            await cloudBlockBlob.UploadFromFileAsync(filePath, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task UploadBlockBlobAsync(string containerName, string blobName, Stream stream)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);

            await cloudBlockBlob.UploadFromStreamAsync(stream, _accesCondition, _blobRequestOptions, _operationContext);
        }
        #endregion

        #region Page Blob Specific Methods
        public async Task UploadPageBlobAsync(string containerName, string blobName, string filePath)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            await cloudPageBlob.UploadFromFileAsync(filePath, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task UploadPageBlobAsync(string containerName, string blobName, Stream stream)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            await cloudPageBlob.UploadFromStreamAsync(stream, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task CreatePageBlobAsync(string containerName, string blobName, long lenght)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            await cloudPageBlob.CreateAsync(lenght, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task WritePageBlobPagesAsync(string containerName, string blobName, Stream stream, long startOffset = 0)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            await cloudPageBlob.WritePagesAsync(stream, startOffset, null, _accesCondition, _blobRequestOptions, _operationContext);
        }

        public async Task<IAzureStreamWriterBlob> GetPageBlobWriteStreamAsync(string containerName, string blobName, long size)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudPageBlob cloudPageBlob = container.GetPageBlobReference(blobName);

            CloudBlobStream blobStream = await cloudPageBlob.OpenWriteAsync(size, _accesCondition, _blobRequestOptions, _operationContext);

            var azureStreamWriterBlob = new AzureStreamWriterBlob(blobStream);

            return azureStreamWriterBlob;
        }
        #endregion

        #region Private Methods
        private async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync(_blobRequestOptions, _operationContext);
            return container;
        }
        #endregion

    }
}