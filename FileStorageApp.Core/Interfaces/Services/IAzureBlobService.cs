namespace FileStorageApp.Core.Interfaces
{
    public interface IAzureBlobService
    {
        Task<string> UploadBlobAsync(Stream fileStream, string blobName);
        Task<bool> DeleteBlobAsync(string blobName);
        Task<Stream> DownloadBlobAsync(string blobName);
        Task<string> CreateFolderAsync(string folderPath);
    }
}
