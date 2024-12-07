using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FileStorageApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly FileStorageDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(FileStorageDbContext context,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserService> logger, 
            IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            // Create user entity
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                CreatedAt = userDto.CreatedAt
            };

            // Save to database
            await _userRepository.AddAsync(user);

            // Return DTO
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
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

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public string GetCurrentUserEmail()
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentUserId()
        {
            var claims = _httpContextAccessor.HttpContext?.User.Claims;
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrWhiteSpace(userId) ? Guid.Empty : new Guid(userId);
        }
    }
}
