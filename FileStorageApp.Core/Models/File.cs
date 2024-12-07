using System.ComponentModel.DataAnnotations;

namespace FileStorageApp.Core.Models
{
    public class File
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string FileName { get; set; }

        [StringLength(50)]
        public required string FileExtension { get; set; }
        public long FileSize { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? FolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public required string StoragePath { get; set; }

        public User Owner { get; set; }
        public Folder? Folder { get; set; }

        // Navigation properties
        public ICollection<FileVersion> Versions { get; set; }
        public ICollection<Share> Shares { get; set; }
    }
}
