using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly FileStorageDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(FileStorageDbContext context,
            IUserRepository userRepository,
            ILogger<UserService> logger, 
            IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            // Create user entity
            var user = new User
            {
                Email = userDto.Email,
                Username = userDto.Username,
                CreatedAt = userDto.CreatedAt
            };

            // Save to database
            await _userRepository.AddAsync(user);

            // Return DTO
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserAsync(string username)
        {
            try
            {
                // Save to database
                var user = await _userRepository.GetUserByUsernameAsync(username);

                // Return DTO
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public string GetCurrentUserEmail()
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentUserId()
        {
            throw new NotImplementedException();
        }
    }
}
