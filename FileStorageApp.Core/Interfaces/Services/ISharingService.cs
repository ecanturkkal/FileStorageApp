using FileStorageApp.Core.Dtos;

namespace FileStorageApp.Core.Interfaces
{
    public interface ISharingService
    {
        Task<ShareDto> CreateShareAsync(CreateShareRequestDto shareRequest);
        Task<IEnumerable<ShareDto>> GetSharesForResourceAsync(Guid resourceId);
        Task<bool> HasUserResourceAccessPermissions(Guid resourceId, Guid ownerId);
    }
}
