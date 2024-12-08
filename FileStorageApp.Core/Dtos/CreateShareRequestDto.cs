using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Dtos
{
    public class CreateShareRequestDto
    {
        public Guid ResourceId { get; set; }
        public ResourceType ResourceType { get; set; }
        public Guid SharedWithId { get; set; }
        public SharePermission Permission { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
