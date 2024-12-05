using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);
        Task<UserDto> GetUserAsync(string userName);
        Guid GetCurrentUserId();
        string GetCurrentUserEmail();
    }
}
