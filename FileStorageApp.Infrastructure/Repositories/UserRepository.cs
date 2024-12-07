using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FileStorageDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(FileStorageDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user");
                throw new FileStorageException("Failed to add user", ex);
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Where(f => f.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}