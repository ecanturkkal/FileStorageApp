using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;

namespace FileStorageApp.Infrastructure.Services
{
    public class SharingService : ISharingService
    {
        public Task<ShareDto> CreateShareAsync(CreateShareRequestDto shareRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShareDto>> GetSharesForResourceAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeShareAsync(Guid shareId)
        {
            throw new NotImplementedException();
        }
    }
}
