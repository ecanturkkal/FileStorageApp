using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);

    }
}
