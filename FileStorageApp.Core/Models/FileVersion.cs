using System.ComponentModel.DataAnnotations;

namespace FileStorageApp.Core.Models
{
    public class FileVersion
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public required string StoragePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public File File { get; set; }
        public User CreatedBy { get; set; }
    }
}
