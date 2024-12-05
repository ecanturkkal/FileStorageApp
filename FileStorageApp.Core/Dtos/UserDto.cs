using FileStorageApp.Core.Models;
namespace FileStorageApp.Core.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
    }
}
