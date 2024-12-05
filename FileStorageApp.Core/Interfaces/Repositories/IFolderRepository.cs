using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Interfaces
{
    public interface IFolderRepository
    {
        Task<Folder> AddAsync(Folder folder);
        Task UpdateAsync(Folder folder);
        Task<bool> DeleteAsync(Guid folderId);
        Task<IEnumerable<Folder>> GetUserFoldersAsync(Guid userId);
    }
}
