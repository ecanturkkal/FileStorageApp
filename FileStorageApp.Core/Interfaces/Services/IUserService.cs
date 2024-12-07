using FileStorageApp.Core.Dtos;

namespace FileStorageApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);
        Task<UserDto> GetUserByUsernameAsync(string userName);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Guid GetCurrentUserId();
        string GetCurrentUserEmail();
    }
}
