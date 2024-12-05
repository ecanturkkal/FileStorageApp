using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Core.Interfaces
{
    public interface IFileRepository
    {
        Task<File> AddAsync(File file);
        Task UpdateAsync(File file);
        Task<bool> DeleteAsync(Guid fileId);
        Task<File?> GetByIdAsync(Guid fileId);
        Task<IEnumerable<File>> GetUserFilesAsync(Guid userId);
    }
}
