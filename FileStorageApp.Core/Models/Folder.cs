using System.ComponentModel.DataAnnotations;

namespace FileStorageApp.Core.Models
{
    public class Folder
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string Name { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FullDirectory { get; set; }
        public User Owner { get; set; }
        public Folder ParentFolder { get; set; }

        // Navigation properties
        public ICollection<File> Files { get; set; }
        public ICollection<Folder> Subfolders { get; set; }
        public ICollection<Share> Shares { get; set; }
    }
}
