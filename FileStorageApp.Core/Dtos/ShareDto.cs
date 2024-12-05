using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Dtos
{
    public class ShareDto
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public ResourceType ResourceType { get; set; }
        public string SharedByEmail { get; set; }
        public string SharedWithEmail { get; set; }
        public SharePermission Permission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
