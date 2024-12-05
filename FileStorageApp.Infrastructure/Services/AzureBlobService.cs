using Azure.Storage.Blobs;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(BlobContainerClient containerClient, ILogger<AzureBlobService> logger)
        {
            _containerClient = containerClient;
            _logger = logger;
        }

        public async Task<string> UploadBlobAsync(Stream fileStream, string blobName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobName);
                await blobClient.UploadAsync(fileStream, overwrite: true);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading blob");
                throw new FileStorageException("Failed to upload file", ex);
            }
        }

        public async Task<bool> DeleteBlobAsync(string blobName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobName);
                await blobClient.DeleteIfExistsAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blob");
                throw new FileStorageException("Failed to delete file", ex);
            }
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobName);
                var downloadResponse = await blobClient.DownloadAsync();
                return downloadResponse.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading blob");
                throw new FileStorageException("Failed to download file", ex);
            }
        }

        public Task<string> CreateFolderAsync(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
