namespace FileStorageApp.Core.Dtos
{
    public class CreateUserDto
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLoginAt { get; set; }
    }
}
