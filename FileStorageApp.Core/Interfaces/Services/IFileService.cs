using FileStorageApp.Core.Dtos;

namespace FileStorageApp.Core.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> UploadFileAsync(Stream fileStream, string fileName, long fileSize, string? folderPath);
        Task<FileDto> GetFileMetadataAsync(Guid fileId);
        Task<Stream> DownloadFileAsync(Guid fileId);
        Task<bool> DeleteFileAsync(Guid fileId);
        Task<IEnumerable<FileVersionDto>> GetFileVersionsAsync(Guid fileId);
    }
}
