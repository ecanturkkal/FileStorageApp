using System.ComponentModel.DataAnnotations;

namespace FileStorageApp.Core.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string Username { get; set; }

        [StringLength(100)]
        public required string Email { get; set; }

        [StringLength(100)]
        public required string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }

        // Navigation properties
        public ICollection<File> Files { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<Share> Shares { get; set; }
    }
}
