using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Interfaces
{
    public interface IShareRepository
    {
        Task<Share> AddAsync(Share share);
        Task<IEnumerable<Share>> GetSharesByResourceIsAsync(Guid resourceId);
    }
}
