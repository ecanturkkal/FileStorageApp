using FileStorageApp.Core.Dtos;

namespace FileStorageApp.Core.Interfaces
{
    public interface IFolderService
    {
        Task<FolderDto> CreateFolderAsync(CreateFolderDto folderDto);
        Task<FolderDetailsDto> GetFolderDetailsAsync(Guid folderId);
        Task<bool> DeleteFolderAsync(Guid folderId);
        Task<IEnumerable<FolderDto>> GetUserFoldersAsync(Guid userId);
    }
}
