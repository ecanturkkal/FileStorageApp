using System.ComponentModel.DataAnnotations;

namespace FileStorageApp.Core.Models
{
    public class Share
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; } // Can be FileId or FolderId
        public ResourceType ResourceType { get; set; }
        public Guid SharedById { get; set; }
        public User SharedBy { get; set; }
        public Guid SharedWithId { get; set; }
        public User SharedWith { get; set; }
        public SharePermission Permission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public enum ResourceType
    {
        File,
        Folder
    }

    public enum SharePermission
    {
        None = 0,
        View = 1,
        Edit = 2,
        Owner = 3
    }
}
