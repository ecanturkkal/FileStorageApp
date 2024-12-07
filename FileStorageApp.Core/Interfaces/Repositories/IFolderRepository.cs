using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Interfaces
{
    public interface IFolderRepository
    {
        Task<Folder> AddAsync(Folder folder);
        Task<bool> DeleteAsync(Guid folderId);
        Task<Folder?> GetByIdAsync(Guid folderId);
        Task<Folder?> GetFolderByNameAsync(string name);
        Task<Folder?> GetFolderByStoragePathAsync(string storagePath);
    }
}
